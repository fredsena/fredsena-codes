

How do I use a jQuery $(document).ready and an ASP.NET UpdatePanel together?

http://stackoverflow.com/questions/1750368/how-do-i-use-a-jquery-document-ready-and-an-asp-net-updatepanel-together?rq=1

<script type="text/javascript">
    $(function() {
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
    });

    function EndRequestHandler(sender, args) {
        // code that you want to run when the request is complete
    }
<script>
