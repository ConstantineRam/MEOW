﻿#if UNITY_IOS
using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using EasyMobile.iOS.Foundation;
using EasyMobile.iOS.GameKit;
using EasyMobile.iOS.UIKit;
using EasyMobile.Internal.GameServices.iOS;

namespace EasyMobile.Internal.GameServices
{
    using GKMatchmakerViewControllerDelegate = GKMatchmakerViewController.GKMatchmakerViewControllerDelegate;
    using GKMatchDelegate = GKMatch.GKMatchDelegate;
    using GKPlayerConnectionState = GKMatch.GKPlayerConnectionState;

    /// <summary>
    /// Game Center real-time multiplayer client.
    /// </summary>
    internal class GCRealTimeMultiplayerClient : IRealTimeMultiplayerClient
    {
        private GKMatch mCurrentMatch;
        private GKMatchmakerViewController mCurrentMatchmakerVC;
        private IRealTimeMultiplayerListener mCurrentListener;

        #region IRealTimeMultiplayerClient

        /*******************************
         * iOS impl:
         *      -> new GKMatchRequest (playersToInvite = nil, allow variant & bitmask exclusive settings) 
         *      -> GKMatchmaker.findMatchForRequest
         *      -> register the listener with the match.delegate -> invoke it when event comes
         * 
         * Android impl:
         *      -> create match request -> call appropriate GPGS methods to set variant and exclusiveBitmask
         *******************************/
        public void CreateQuickMatch(MatchRequest request, IRealTimeMultiplayerListener listener)
        {
            Util.NullArgumentTest(request);
            Util.NullArgumentTest(listener);

            using (var gkReq = request.ToGKMatchRequest())
            {
                GKMatchmaker.SharedMatchmaker().FindMatchForRequest(gkReq, (gkMatch, nsError) =>
                    {
                        // If new match is created successfully, store it and the given listener.
                        if (gkMatch != null)
                            SetupCurrentMatchAndListener(gkMatch, listener);

                        RuntimeHelper.RunOnMainThread(() =>
                            {
                                ReportRoomSetupProgress(listener, gkMatch, nsError);
                            });
                    });
            }
        }

        /*******************************
         * iOS impl:
         *      -> new GKMatchRequest (allow variant & bitmask exclusive settings) 
         *      -> show GKMatchmakerVC
         *      -> register the listener with the MatchmakerDelegate -> invoke it when event comes
         * 
         * Android impl:
         *      -> create match request -> call appropriate GPGS methods to set variant and exclusiveBitmask
         *******************************/
        public void CreateWithMatchmakerUI(MatchRequest request, IRealTimeMultiplayerListener listener)
        {
            Util.NullArgumentTest(request);
            Util.NullArgumentTest(listener);

            if (mCurrentMatchmakerVC != null)
            {
                Debug.Log("Ignoring CreateWithMatchmakerUI call because another matchmaker UI is being shown.");
                return;
            }

            // Create a new GKMatchmakerViewController.
            var vc = InteropObjectFactory<GKMatchmakerViewController>.Create(
                         () =>
                {
                    using (var gkReq = request.ToGKMatchRequest())
                        return new GKMatchmakerViewController(gkReq);
                },
                         viewController =>
                {
                    return viewController.ToPointer();
                });

            // Create a delgate for the vc.
            vc.MatchmakerDelegate = new InternalGKMatchmakerViewControllerDelegateImpl(this, listener);

            // Store the VC ref.
            mCurrentMatchmakerVC = vc;

            // Now show the VC.
            using (var unityVC = UIViewController.UnityGetGLViewController())
                unityVC.PresentViewController(vc, true, null);
        }

        /*******************************
         * * TO BE TESTED to make sure GC has a similar UI
         * 
         * TENTATIVE IMPL SUGGESTION:
         * iOS impl:
         *      -> new dummy GKMatchRequest 
         *      -> show GKMatchmakerVC
         *      -> register listener with the MatchmakerDelegate
         * 
         * Android impl:
         *      -> call GPGS method
         *******************************/
        /// <summary>
        /// Creates a real-time game starting with the inbox screen.
        /// </summary>
        /// <remarks>On the inbox screen,
        /// the player can select an invitation to accept, in which case the room setup
        /// process will start. The listener's
        /// <see cref="RealTimeMultiplayerListener.OnRoomSetupProgress"/> will be called
        /// to report room setup progress, and eventually
        /// <see cref="RealTimeMultiplayerListener.OnRoomConnected"/> will be called to
        /// indicate that the room setup is either complete or has failed (check the
        /// <b>success</b> parameter of the callback).
        /// </remarks>
        /// <param name="listener">Listener. The listener to notify of relevant events.</param>
        public void ShowInvitationsUI(IRealTimeMultiplayerListener listener)
        {
            Debug.Log("ShowInvitationsUI is not available on Game Center platform.");
        }

        /// <summary>
        /// Accepts an invitation.
        /// </summary>
        /// <remarks>This will not show any UI. The listener's
        /// <see cref="RealTimeMultiplayerListener.OnRoomSetupProgress"/> will be called
        /// to report room setup progress, and eventually
        /// <see cref="RealTimeMultiplayerListener.OnRoomConnected"/> will be called to
        /// indicate that the room setup is either complete or has failed (check the
        /// <b>success</b> parameter of the callback).
        /// </remarks>
        /// <param name="invitation">Invitation to accept.</param>
        /// <param name="showWaitingRoomUI">Whether to show the waiting room UI.</param>
        /// <param name="listener">Listener. Listener to notify of relevant events.</param>
        public void AcceptInvitation(Invitation invitation, bool showWaitingRoomUI, IRealTimeMultiplayerListener listener)
        {
            Util.NullArgumentTest(invitation);
            Util.NullArgumentTest(listener);

            if (showWaitingRoomUI)
            {
                // Close the current matchmakerVC if any.
                if (mCurrentMatchmakerVC != null)
                {
                    mCurrentMatchmakerVC.DismissViewController(true, null);
                    mCurrentMatchmakerVC = null;
                }

                // Create a new GKMatchmakerViewController from the invitation.
                var vc = InteropObjectFactory<GKMatchmakerViewController>.Create(
                             () => new GKMatchmakerViewController(invitation.GK_Invite),
                             viewController => viewController.ToPointer()
                         );

                // Create a delgate for the vc.
                vc.MatchmakerDelegate = new InternalGKMatchmakerViewControllerDelegateImpl(this, listener);

                // Store the VC ref.
                mCurrentMatchmakerVC = vc;

                // Now show the VC.
                using (var unityVC = UIViewController.UnityGetGLViewController())
                    unityVC.PresentViewController(vc, true, null);
            }
            else
            {
                // Create a GKMatch from the invitation without any UI.
                GKMatchmaker.SharedMatchmaker().MatchForInvite(invitation.GK_Invite, (gkMatch, nsError) =>
                    {
                        // If new match is created successfully, store it and the given listener.
                        if (gkMatch != null)
                            SetupCurrentMatchAndListener(gkMatch, listener);

                        RuntimeHelper.RunOnMainThread(() =>
                            {
                                ReportRoomSetupProgress(listener, gkMatch, nsError);
                            });
                    });
            }
        }

        /*******************************
         * TENTATIVE IMPL SUGGESTION:
         * iOS impl:
         *      -> GKMatch:sendDataToAllPlayers:withDataMode
         * 
         * Android impl:
         *      -> call GPGS method
         *******************************/
        /// <summary>Sends a message to all other participants.</summary>
        /// <param name="reliable">If set to <c>true</c>, mesasge is reliable; if not,
        /// it is unreliable. Unreliable messages are faster, but are not guaranteed to arrive
        /// and may arrive out of order.</param>
        /// <param name="data">Data. The data to send.</param>
        public void SendMessageToAll(bool reliable, byte[] data)
        {
            Util.NullArgumentTest(data);

            if (mCurrentMatch != null)
            {
                NSError error;
                mCurrentMatch.SendDataToAllPlayers(
                    data,
                    reliable ? GKMatch.GKMatchSendDataMode.Reliable : GKMatch.GKMatchSendDataMode.Unreliable,
                    out error);
            }
        }

        /*******************************
         * TENTATIVE IMPL SUGGESTION:
         * iOS impl:
         *      Extract the data needed based on offset and length -> GKMatch:sendDataToAllPlayers:withDataMode
         * 
         * Android impl:
         *      -> call GPGS method
         *******************************/
        /// <summary>
        /// Same as <see cref="SendMessageToAll(bool,byte[])" />, but allows you to specify
        /// offset and length of the data buffer.
        /// </summary>
        /// <param name="reliable">If set to <c>true</c>, message is reliable.</param>
        /// <param name="data">Data.</param>
        /// <param name="offset">Offset. Offset of the data buffer where data starts.</param>
        /// <param name="length">Length. Length of data (from offset).</param>
        public void SendMessageToAll(bool reliable, byte[] data, int offset, int length)
        {
            SendMessageToAll(reliable, new List<byte>(data).GetRange(offset, length).ToArray());
        }

        /*******************************
         * TENTATIVE IMPL SUGGESTION:
         * iOS impl:
         *      -> GKMatch:sendData:toPlayers:dataMode
         * 
         * Android impl:
         *      -> call GPGS method
         *******************************/
        /// <summary>
        /// Send a message to a particular participant.
        /// </summary>
        /// <param name="reliable">If set to <c>true</c>, message is reliable; if not,
        /// it is unreliable. Unreliable messages are faster, but are not guaranteed to arrive
        /// and may arrive out of order.</param>
        /// <param name="participantId">Participant ID. The participant to whom the message
        /// will be sent.</param>
        /// <param name="data">Data. The data to send.</param>
        public void SendMessage(bool reliable, string participantId, byte[] data)
        {
            Util.NullArgumentTest(participantId);
            Util.NullArgumentTest(data);

            if (mCurrentMatch == null)
                return;

            // Cache the NSArray of players.
            var playersNSArray = mCurrentMatch.Players;

            if (playersNSArray == null)
                return;

            var players = playersNSArray.ToArray(p => InteropObjectFactory<GKPlayer>.FromPointer(p, ptr => new GKPlayer(ptr)));
            var player = players.FirstOrDefault(p => p.PlayerID.Equals(participantId));

            if (player == default(GKPlayer))
                return;

            NSError error;
            mCurrentMatch.SendData(
                data,
                new GKPlayer[]{ player },
                reliable ? GKMatch.GKMatchSendDataMode.Reliable : GKMatch.GKMatchSendDataMode.Unreliable,
                out error);
        }

        /*******************************
        * TENTATIVE IMPL SUGGESTION:
        * iOS impl:
        *      Extract the data needed based on offset and length -> GKMatch:sendData:toPlayers:dataMode
        * 
        * Android impl:
        *      -> call GPGS method
        *******************************/
        /// <summary>
        /// Same as <see cref="SendMessage(bool,string,byte[])" />, but allows you to specify
        /// the offset and length of the data buffer.
        /// </summary>
        public void SendMessage(bool reliable, string participantId, byte[] data, int offset, int length)
        {
            SendMessage(reliable, participantId, new List<byte>(data).GetRange(offset, length).ToArray());
        }

        /*******************************
         * TENTATIVE IMPL SUGGESTION:
         * iOS impl:
         *      -> GKMatch:players -> create dummy participant to wrap each player
         * 
         * Android impl:
         *      -> call GPGS method
         *******************************/
        /// <summary>Gets the connected participants, including self.</summary>
        /// <returns>The connected participants, including self. This list is guaranteed
        /// to be ordered lexicographically by Participant ID, which means the ordering will be
        /// the same to all participants.</returns>
        public List<Participant> GetConnectedParticipants()
        {
            // If no match is connected, simply return null.
            if (mCurrentMatch == null)
                return null;

            var list = new List<Participant>();

            // First grab the connected peers.
            var playersNSArray = mCurrentMatch.Players;
            if (playersNSArray != null)
            {
                var players = playersNSArray.ToArray(p => InteropObjectFactory<GKPlayer>.FromPointer(p, ptr => new GKPlayer(ptr)));
                list.AddRange(players.Select(p => Participant.FromGKPlayer(p, Participant.ParticipantStatus.Joined, true)));
            }

            // Add the local player (self) as GameKit doesn't include this.
            list.Add(Participant.FromGKPlayer(
                    GKLocalPlayer.LocalPlayer, 
                    Participant.ParticipantStatus.Joined, 
                    true));

            // Sort the participants based on their ID and return the sorted list.
            return list.OrderBy(p => p.ParticipantId).ToList();
        }

        /*******************************
         * TENTATIVE IMPL SUGGESTION:
         * iOS impl:
         *      -> GKMatch:players (check ID to find self) -> create dummy participant to wrap the player
         * 
         * Android impl:
         *      -> call GPGS method
         *******************************/
        /// <summary>Gets the participant that represents the current player.</summary>
        /// <returns>Self.</returns>
        public Participant GetSelf()
        {
            return Participant.FromGKPlayer(
                GKLocalPlayer.LocalPlayer, 
                Participant.ParticipantStatus.Joined, 
                true);
        }

        /*******************************
         * TENTATIVE IMPL SUGGESTION:
         * iOS impl:
         *      -> GKMatch:players (check ID to find required participant)
         *      -> create dummy participant to wrap the player
         * 
         * Android impl:
         *      -> call GPGS method
         *******************************/
        /// <summary>Gets a participant by ID.</summary>
        /// <returns>The participant, or <c>null</c> if not found.</returns>
        /// <param name="participantId">Participant id.</param>
        public Participant GetParticipant(string participantId)
        {
            if (string.IsNullOrEmpty(participantId))
                return null;

            if (mCurrentMatch == null)
                return null;

            var playersNSArray = mCurrentMatch.Players;

            if (playersNSArray == null)
                return null;

            var players = playersNSArray.ToArray(p => InteropObjectFactory<GKPlayer>.FromPointer(p, ptr => new GKPlayer(ptr)));

            return Participant.FromGKPlayer(
                players.FirstOrDefault(p => p.PlayerID.Equals(participantId)), 
                Participant.ParticipantStatus.Joined, 
                true);
        }

        /*******************************
         * TENTATIVE IMPL SUGGESTION:
         * iOS impl:
         *      -> GKMatch:disconnect
         * 
         * Android impl:
         *      -> call GPGS method
         *******************************/
        public void LeaveRoom()
        {
            if (mCurrentMatch != null)
            {
                // Disconnect the match.
                mCurrentMatch.Disconnect();

                // Nil out the delegate as recommended by GameKit.
                // https://developer.apple.com/documentation/gamekit/gkmatch?language=objc
                mCurrentMatch.Delegate = null;

                if (mCurrentListener != null)
                    mCurrentListener.OnLeftRoom();

                // Reset current match and listener.
                mCurrentMatch.Dispose();
                mCurrentMatch = null;
                mCurrentListener = null;
            }
        }

        /*******************************
         * TENTATIVE IMPL SUGGESTION:
         * iOS impl:
         *      -> GKMatch:expectedPlayerCount == 0?
         * 
         * Android impl:
         *      -> call GPGS method
         *******************************/
        public bool IsRoomConnected()
        {
            return mCurrentMatch != null && mCurrentMatch.ExpectedPlayerCount == 0;
        }

        public void DeclineInvitation(Invitation invitation)
        {
            // No-op method on GC.
        }

        #endregion

        #region Helpers

        private void SetupCurrentMatchAndListener(GKMatch match, IRealTimeMultiplayerListener listener)
        {
            mCurrentMatch = match;
            mCurrentListener = listener;

            if (mCurrentMatch != null)
                mCurrentMatch.Delegate = new InternalGKMatchDelegateImpl(this);
        }

        private void ResetCurrentMatchmakerVC()
        {
            mCurrentMatchmakerVC = null;
        }

        private static void ReportRoomSetupProgress(IRealTimeMultiplayerListener listener, GKMatch match, NSError error, bool programmaticMatchmaking = false)
        {
            if (listener == null)
                return;

            // Setup failed.
            if (match == null || error != null)
            {
                listener.OnRoomConnected(false);
                return;
            }

            // Setup succeeded or is progressing.
            float progress;
            bool completed = IsRoomSetupComplete(match, out progress);

            // On progress.
            listener.OnRoomSetupProgress(progress);

            // Connected.
            if (completed)
            {
                // Programmatic matchmaking has finished.
                if (programmaticMatchmaking)
                    GKMatchmaker.SharedMatchmaker().FinishMatchmakingForMatch(match);
                
                listener.OnRoomConnected(true);
            }
        }

        private static bool IsRoomSetupComplete(GKMatch match, out float progress)
        {
            if (match == null)
            {
                progress = 0;
                return false;
            }

            // Setup succeeded or is progressing.
            var players = match.Players;
            uint joinedCount = players != null ? players.Count : 0;
            uint expectedCount = match.ExpectedPlayerCount;

            // GameKit doesn't count the local player as a joined player,
            // so we account for it by +1 manually.
            joinedCount++;

            // Now calculate the progress based on the number of joined players 
            // compared to the total players needed.
            progress = joinedCount * 100f / (joinedCount + expectedCount);

            return expectedCount == 0;
        }

        #endregion

        #region Internal Delegates

        /// <summary>
        /// Internal GKMatchmakerViewController delegate. Used when creating a match with the default UI.
        /// </summary>
        private class InternalGKMatchmakerViewControllerDelegateImpl : GKMatchmakerViewControllerDelegateImpl
        {
            private GCRealTimeMultiplayerClient mClient;
            private IRealTimeMultiplayerListener mListener;

            public InternalGKMatchmakerViewControllerDelegateImpl(GCRealTimeMultiplayerClient client, IRealTimeMultiplayerListener listener)
            {
                Util.NullArgumentTest(client);

                this.mClient = client;
                this.mListener = listener;
            }

            public override void MatchmakerViewControllerDidFindMatch(GKMatchmakerViewController viewController, GKMatch match)
            {
                // Automatically close the VC.
                if (viewController != null)
                    viewController.DismissViewController(true, null);

                mClient.ResetCurrentMatchmakerVC();
                
                if (match == null)  // should never happen
                    return;

                // Set the newly created match as the current one.
                mClient.SetupCurrentMatchAndListener(match, mListener);

                // Report room setup completed.
                RuntimeHelper.RunOnMainThread(() =>
                    {
                        ReportRoomSetupProgress(mListener, match, null);
                    });
            }

            public override void MatchmakerViewControllerWasCancelled(GKMatchmakerViewController viewController)
            {
                // Close the VC.
                if (viewController != null)
                    viewController.DismissViewController(true, null);

                mClient.ResetCurrentMatchmakerVC();
            }

            public override void MatchmakerViewControllerDidFailWithError(GKMatchmakerViewController viewController, NSError error)
            {
                Debug.Log("MatchmakerViewControllerDidFailWithError: " + (error != null ? error.LocalizedDescription : "null"));
            }
        }

        /// <summary>
        /// Internal GKMatch delegate. Used when a match is created.
        /// </summary>
        private class InternalGKMatchDelegateImpl : GKMatchDelegateImpl
        {
            private GCRealTimeMultiplayerClient mClient;

            public InternalGKMatchDelegateImpl(GCRealTimeMultiplayerClient client)
            {
                Util.NullArgumentTest(client);
                this.mClient = client;
            }

            public override void PlayerDidChangeConnectionState(GKMatch match, GKPlayer player, GKPlayerConnectionState state)
            {
                RuntimeHelper.RunOnMainThread(() =>
                    {
                        var listener = mClient.mCurrentListener;

                        if (listener == null || match == null || player == null)
                            return;
                
                        if (player.Equals(GKLocalPlayer.LocalPlayer))
                        {
                            // In reality, this never runs because GameKit never reports 
                            // connection state changes of local player.
                            if (state == GKPlayerConnectionState.Disconnected)
                                listener.OnLeftRoom();
                        }
                        else
                        {
                            if (state == GKPlayerConnectionState.Connected)
                            {
                                listener.OnPeersConnected(new string[] { player.PlayerID });
                                ReportRoomSetupProgress(listener, match, null, true);
                            }
                            else if (state == GKPlayerConnectionState.Disconnected)
                            {
                                if (match.ExpectedPlayerCount > 0)  // player leaves during room setup
                                {
                                    listener.OnParticipantLeft(Participant.FromGKPlayer(player, state.ToParticipantStatus(), false));
                                    ReportRoomSetupProgress(listener, match, null, true);
                                }
                                else    // player disconnected after match setup has completed
                                {
                                    listener.OnPeersDisconnected(new string[] { player.PlayerID });
                                }
                            }
                        }
                    });
            }

            public override void MatchDidReceiveDataForRecipient(GKMatch match, NSData data, GKPlayer recipient, GKPlayer remotePlayer)
            {
                if (recipient != null && recipient.Equals(GKLocalPlayer.LocalPlayer))
                {
                    MatchDidReceiveData(match, data, remotePlayer);
                }
            }

            public override void MatchDidReceiveData(GKMatch match, NSData data, GKPlayer remotePlayer)
            {
                var listener = mClient.mCurrentListener;
                if (listener == null || match == null || data == null || remotePlayer == null)
                    return;

                var bytes = data.ToBytes();
                RuntimeHelper.RunOnMainThread(() =>
                    {
                        listener.OnRealTimeMessageReceived(remotePlayer.PlayerID, bytes);
                    });
            }

            public override bool ShouldReinviteDisconnectedPlayer(GKMatch match, GKPlayer player)
            {
                var listener = mClient.mCurrentListener;
                if (listener == null || match == null || player == null)
                    return false;

                return listener.ShouldReinviteDisconnectedPlayer(Participant.FromGKPlayer(
                        player,
                        GKPlayerConnectionState.Disconnected.ToParticipantStatus(),
                        false
                    ));
            }
        }

        #endregion
    }
}
#endif