<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="ReservationByDate.aspx.cs" Inherits="SamplePages_ReservationFormsByDate" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <h1>Reservations By Date (Repeater)</h1>
        <table align="center" style="width: 70%">
        <tr>
            <td align="right">Enter Reservation date (mm/dd/yyy):</td>
            <td>
                <asp:TextBox ID="ReservationDateArg" runat="server"
                    ToolTip="mm//dd/yyyy" Text="01/01/1990"></asp:TextBox>
                &nbsp;
                <asp:LinkButton ID="FetchReservations" runat="server">Fetch Reservations</asp:LinkButton>
            </td>
        </tr>
        <tr> 
            <td colspan="2">&nbsp;</td>
        </tr>
        <tr>
            <td colspan="2">
                <div class="row col-md-12">
                    <asp:Repeater ID="EventReservations" runat="server" 
                        DataSourceID="ODSReservations">
                        <ItemTemplate>
                            <h3><%# Eval("Description") %></h3>
                            <asp:Repeater ID="ReservationList" runat="server"
                                DataSource ='<%# Eval("Reservations") %>'>
                                <ItemTemplate>
                                    <h5>
                                        <%# Eval("CustomerName") %>
                                        <%# Eval("ContactPhone") %>
                                    </h5>
                                </ItemTemplate>
                            </asp:Repeater>
                        </ItemTemplate>
                    </asp:Repeater>

                </div>
            </td>
            
        </tr>
        </table>
        
    <asp:ObjectDataSource ID="ODSReservations" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="GetReservationsByDate" TypeName="eRestaurantSystem.BLL.AdminController">
        <SelectParameters>
            <asp:ControlParameter 
                ControlID="ReservationDateArg" 
                Name="reservationDate" 
                PropertyName="Text" 
                Type="String" />
        </SelectParameters>
    </asp:ObjectDataSource>
</asp:Content>

