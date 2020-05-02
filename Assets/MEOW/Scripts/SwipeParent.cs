using System;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class SwipeParent : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
  protected int SwipeDelta = 10;
  private Vector2 StartingCoords;
  private bool dragging = false;

  //---------------------------------------------------------------------------------------------------------------
  public void OnDrag(PointerEventData eventData)
  {
    if (this.dragging)
    {
      return;
    }
    this.dragging = true;
    this.StartingCoords = eventData.position;

  }

  //---------------------------------------------------------------------------------------------------------------
  public void OnBeginDrag(PointerEventData eventData)
  {
    Debug.Log("piu 0ne");
    if (this.dragging)
    {
      return;
    }

    this.StartingCoords = eventData.position;
    this.dragging = true;
  }

  //---------------------------------------------------------------------------------------------------------------
  protected void OnSwipeLeft()
  {
    Game.Events.SwipeLeft.Invoke();
  }

  //---------------------------------------------------------------------------------------------------------------
  protected void OnSwipeRight()
  {
    Game.Events.SwipeRight.Invoke();
  }

  //---------------------------------------------------------------------------------------------------------------
  public void OnEndDrag(PointerEventData eventData)
  {
 
    this.dragging = false;
    Vector2 newPos = eventData.position;

    float swipeDifference = newPos.x - this.StartingCoords.x;

    if (Math.Abs( swipeDifference) < this.SwipeDelta)
    {
      return;
    }

    if (swipeDifference < 0)
    {
      this.OnSwipeRight();
      
      return;
    }

    this.OnSwipeLeft();
  }

 }
