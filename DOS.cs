
//Search inside files to remove TFS settings in order to make project offline:
*.csproj + Scc
*.vssscc
.vspscc + scc

Import-Module WebAdministration:
Set-ItemProperty IIS:\sites\[site name] -Name enabledProtocols -Value "http,net.pipe,net.tcp"
Set-ItemProperty IIS:\Sites\[site name] -Name bindings -Value @(@{protocol="net.tcp"; bindingInformation="812:*"}, @{protocol="net.pipe"; bindingInformation="[url name]"}) -Force
 

//Merge a text (.txt) file in the Windows command line
copy *.txt newfile.txt

//Recursively copy files of a specific pattern into a single flat file on Windows
mkdir targetDir
for /r %x in (*.cs) do copy "%x" targetDir\ /Y
cd targetDir
copy *.cs newfile.txt

//Remove Connected user in Remote Desktop Connection
//https://superuser.com/questions/585922/remove-connected-user-in-remote-desktop-connection
mstsc /v:n.n.n.n /admin

/*
To connect to the Terminal Server despite the limited connections, what you can do is to connect to the server with the /admin switch. To do this, launch mstsc as follows:

mstsc /v:n.n.n.n /admin
Replace n.n.n.n with the IP of the server and you’re good to go. Once connected to the server, use Task Manager to log off the sessions that are no longer used.

To do this, select the session you wish to disconnect and click Logoff. Note that this will effectively log off the session and closes all windows still open in that session!

Once these steps have been completed, log your current session off and you should be able to reconnect to the server without using the /admin switch.
*/
