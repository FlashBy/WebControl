<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Register assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" namespace="System.Web.UI.DataVisualization.Charting" tagprefix="asp" %>

<asp:Content ID="indexTitle" ContentPlaceHolderID="TitleContent" runat="server">
    Home Page - My ASP.NET MVC Application
</asp:Content>

<asp:Content ID="indexFeatured" ContentPlaceHolderID="FeaturedContent" runat="server">
    <section class="featured">
        <div class="content-wrapper">
            <hgroup class="title">
                <h1>Home Page.</h1>
                <h2><%: ViewBag.Message %></h2>
            </hgroup>
            <p>
                To learn more about ASP.NET MVC visit
                <a href="http://asp.net/mvc" title="ASP.NET MVC Website">http://asp.net/mvc</a>.
                The page features <mark>videos, tutorials, and samples</mark> to help you get the most from ASP.NET MVC.
                If you have any questions about ASP.NET MVC visit
                <a href="http://forums.asp.net/1146.aspx/1?MVC" title="ASP.NET MVC Forum">our forums</a>.
            </p>
        </div>
    </section>
</asp:Content>

<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
    <form id="form1" runat="server">
    <asp:Chart ID="Chart1" runat="server">
        <series>
            <asp:Series Name="Series1">
            </asp:Series>
        </series>
        <chartareas>
            <asp:ChartArea Name="ChartArea1">
            </asp:ChartArea>
        </chartareas>
    </asp:Chart>
    </form>

    <p>
    Page Rendered: <%= DateTime.Now.ToLongTimeString() %>
</p>
<span id="status">No Status</span>
<br />   
<%= Ajax.ActionLink("Update Status", "GetStatus", new AjaxOptions{UpdateTargetId="status" }) %>
<br /><br />
<% using(Ajax.BeginForm("UpdateForm", new AjaxOptions{UpdateTargetId="textEntered"})) { %>
  <%= Html.TextBox("textBox1","Enter text")%>  
  <input type="submit" value="Submit"/><br />
  <span id="textEntered">Nothing Entered</span>
<% } %>

<script type="text/javascript">
   
    function ShowTime() {
        var myDate = new Date();
        var label1 = document.getElementById("<%= Label1.ClientID %>");
        label1.innerText = myDate.toLocaleString();
    }
    setInterval("ShowTime();", 2000); //如2000表示2秒请求一次
</script>

 <asp:Label ID="Label1" runat="server" Text="Label" Width="289px"></asp:Label>   

</asp:Content>



