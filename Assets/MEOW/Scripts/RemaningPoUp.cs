using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RemaningPoUp : SharingPopUp
{
  [SerializeField]
  private Button RenameButton;
  [SerializeField]
  private InputField RenameKotanField;

  [SerializeField]
  private Button ChangeQuoteButton;
  [SerializeField]
  private InputField ChangeQuoteField;

  private const int FOUND_NOTHING = -1;
  private const string TAP_TO_NAME = "Tap to Name";

  protected CustomKotamData myCurrentData;

  //---------------------------------------------------------------------------------------------------------------
  protected void PrepareNamingFields()
  {
    if (this.myCurrentData.GetName == CustomKotamData.NO_NAME)
    {
      this.RenameKotanField.text = "Tap to Name";
    }
    else
    {
      this.RenameKotanField.text = this.myCurrentData.GetName;
    }
    this.ChangeQuoteField.text = this.myCurrentData.GetQuote;

  }

  //---------------------------------------------------------------------------------------------------------------
  protected void DisableAndHideNamingFields()
  {
    this.ChangeQuoteButton.interactable = false;
    this.ChangeQuoteField.interactable = false;
    this.ChangeQuoteField.text = "";

    this.ChangeQuoteButton.interactable = false;
    this.RenameButton.interactable = false;

    this.RenameKotanField.interactable = false;
    this.RenameKotanField.text = "";
  }

  //---------------------------------------------------------------------------------------------------------------
  public virtual void OnQuoteChangeFinished()
  {
    if (this.ChangeQuoteField.text == CustomKotamData.NO_NAME)
    {
      return;
    }
    Debug.Log(this.ChangeQuoteField.text);

    this.myCurrentData.SetQuote = this.ChangeQuoteField.text;
    this.ChangeQuoteButton.interactable = true;
    this.RenameButton.interactable = true;

    this.ChangeQuoteField.interactable = false;

  }

  //---------------------------------------------------------------------------------------------------------------
  public virtual void OnChangeQuoteClicked()
  {
    if (this.myCurrentData.GetQuote == CustomKotamData.NO_NAME)
    {
      this.ChangeQuoteField.text = "";
    }

    this.ChangeQuoteButton.interactable = false;
    this.RenameButton.interactable = false;

    this.ChangeQuoteField.interactable = true;

    this.ChangeQuoteField.ActivateInputField();
    this.ChangeQuoteField.Select();


  }

  //---------------------------------------------------------------------------------------------------------------
  public virtual void OnRenameFinished()
  {
    if (this.RenameKotanField.text == CustomKotamData.NO_NAME)
    {
      this.RenameKotanField.text = TAP_TO_NAME;
      return;
    }

    this.myCurrentData.SetName = this.RenameKotanField.text;
    this.ChangeQuoteButton.interactable = true;
    this.RenameButton.interactable = true;

    this.RenameKotanField.interactable = false;

  }



  //---------------------------------------------------------------------------------------------------------------
  public virtual void OnRenameClicked()
  {
    if (this.myCurrentData.GetName == CustomKotamData.NO_NAME)
    {
      this.RenameKotanField.text = "";
    }

    this.ChangeQuoteButton.interactable = false;
    this.RenameButton.interactable = false;

    this.RenameKotanField.interactable = true;

    this.RenameKotanField.ActivateInputField();
    this.RenameKotanField.Select();
  }


}
