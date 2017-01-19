
//Script Task: Change framework to 4.5: (Right-click on project, properties)
using System.IO;
using System.IO.Compression;

public void Main()
{
	string destPath = Dts.Variables["User::ProcessingPath"].Value.ToString();
	destPath = destPath.LastIndexOf(@"\") == -1 ? destPath : destPath.Substring(0, destPath.Length - 1);

	string FileName = Dts.Variables["User::FileName"].Value.ToString();            

	string ZipFileName = Path.GetFileNameWithoutExtension(FileName) + ".zip";            

	string zipPath = Path.Combine(Dts.Variables["User::ProcessedFile"].Value.ToString(), ZipFileName);

	//IMPORTANT: The zip file's destination folder cannot be the same destination from the file that will be zipped. Cause errors!!!!
	System.IO.Compression.ZipFile.CreateFromDirectory(destPath, zipPath);

	Dts.Variables["User::OutputZipFile"].Value = zipPath;

	Dts.TaskResult = (int)ScriptResults.Success;
}
