<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="SpecialEventsAdmin.aspx.cs" Inherits="SamplePages_SpecialEventsAdmin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <h1>Special Events Administration</h1>
    <p>&nbsp;</p>
    <table style="width: 80%">
        <tr>
            <td align="right" style="width:50%">Select an Event: &nbsp;&nbsp;</td>
            <td>
                <asp:DropDownList ID="SpecialEventList" runat="server"
                    width="200px" DataSourceID="ODSSpecialEvents" DataTextField="Description" DataValueField="EventCode" AppendDataBoundItems="true">
                    <asp:ListItem Value="z">Select Event</asp:ListItem>
                </asp:DropDownList>&nbsp;&nbsp;
                <asp:LinkButton ID="FetchReservations" runat="server" OnClick="FetchReservations_Click">Fetch Reservations</asp:LinkButton>
            </td>
        </tr>
        <tr>
            <td colspan="2">&nbsp;</td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:GridView ID="ReservationList" runat="server" AutoGenerateColumns="False" DataSourceID="ODSReservations" PageSize="5" AllowPaging="True">
                    <AlternatingRowStyle BackColor="Silver" />
                    <Columns>
                        <asp:BoundField DataField="CustomerName" HeaderText="Customer Name" SortExpression="CustomerName">
                        <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="ReservationDate" DataFormatString="{0:MMM dd, yyyy}" HeaderText="Date" SortExpression="ReservationDate">
                        <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="NumberInParty" HeaderText="Size" SortExpression="NumberInParty" >
                        <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField DataField="ContactPhone" HeaderText="Phone" SortExpression="ContactPhone" >
                        <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="ReservationStatus" HeaderText="Status" SortExpression="ReservationStatus" >
                        <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                    </Columns>
                    <HeaderStyle BackColor="Gray" Font-Size="Large" HorizontalAlign="Left" />
                    <PagerSettings Mode="NumericFirstLast" />
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:DetailsView ID="ReservationListDV" runat="server" Height="50px" Width="125px" AllowPaging="True" DataSourceID="ODSReservations">
                    <EmptyDataTemplate>
                        Select a Position
                    </EmptyDataTemplate>
                </asp:DetailsView>
            </td>
            
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
        </tr>
    </table>
    <asp:ObjectDataSource ID="ODSSpecialEvents" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="SpecialEvent_List" TypeName="eRestaurantSystem.BLL.AdminController"></asp:ObjectDataSource>
    <asp:ObjectDataSource ID="ODSReservations" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="GetReservationsByEventCode" TypeName="eRestaurantSystem.BLL.AdminController">
        <SelectParameters>
            <asp:ControlParameter ControlID="SpecialEventList" Name="eventcode" PropertyName="SelectedValue" Type="String" />
        </SelectParameters>
    </asp:ObjectDataSource>
</asp:Content>

