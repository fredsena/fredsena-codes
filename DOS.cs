
//Merge a text (.txt) file in the Windows command line
copy *.txt newfile.txt

//Recursively copy files of a specific pattern into a single flat file on Windows
mkdir targetDir
for /r %x in (*.cs) do copy "%x" targetDir\ /Y
cd targetDir
copy *.cs newfile.txt
