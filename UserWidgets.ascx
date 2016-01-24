<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserWidgets.ascx.cs" Inherits="SABB.Website.layouts.SABB.SubLayouts.Home.UserWidgets" %>

<div class="widgets-container">
    <div class="row">
        &nbsp;
    </div>

    <div data-widget-placeholder-order="1" class="column blank added-items">
        <asp:PlaceHolder ID="phWidget1" runat="server"></asp:PlaceHolder>
        &nbsp;
    </div>

    <div class="two-column-container">
        <div class="column" data-widget-placeholder-order="2">
            <asp:PlaceHolder ID="phWidget2" runat="server"></asp:PlaceHolder>
        </div>
        <div class="column" data-widget-placeholder-order="3">
            <asp:PlaceHolder ID="phWidget3" runat="server"></asp:PlaceHolder>
        </div>
    </div>

    <div class="column blank" data-widget-placeholder-order="4">
        &nbsp;
        <asp:PlaceHolder ID="phWidget4" runat="server"></asp:PlaceHolder>
    </div>

    <div class="three-column-container">
        <div class="column three" data-widget-placeholder-order="5">
            <asp:PlaceHolder ID="phWidget5" runat="server"></asp:PlaceHolder>
        </div>
        <div class="column three" data-widget-placeholder-order="6">
            <asp:PlaceHolder ID="phWidget6" runat="server"></asp:PlaceHolder>
        </div>
        <div class="column three" data-widget-placeholder-order="7">
            <asp:PlaceHolder ID="phWidget7" runat="server"></asp:PlaceHolder>
        </div>
    </div>

    <div class="one-column-container">
        <div class="column one" data-widget-placeholder-order="8">
            <asp:PlaceHolder ID="phWidget8" runat="server"></asp:PlaceHolder>
        </div>
    </div>

</div>
