
Example to create RDLC file by hand and c# code for local custom dataset and datasource  based on VS Module Class Project:

Steps:
1. Create an empty rdlc file
2. Edit it on Notepad++ (or on your favorite editor)
3. Replace <DataSources> and <DataSets> with your own objects
4. Test your data using the c# code (included in this page) with a mvc controller.


<Report>
...
...
...

<Page>
    <PageHeight>29.7cm</PageHeight>
    <PageWidth>21cm</PageWidth>
    <LeftMargin>2cm</LeftMargin>
    <RightMargin>2cm</RightMargin>
    <TopMargin>2cm</TopMargin>
    <BottomMargin>2cm</BottomMargin>
    <ColumnSpacing>0.13cm</ColumnSpacing>
    <Style />
  </Page>
  <AutoRefresh>0</AutoRefresh>
  <DataSources>
    <DataSource Name="NameModuleProject">
      <ConnectionProperties>
        <DataProvider>System.Data.DataSet</DataProvider>
        <ConnectString>/* Local Connection */</ConnectString>
      </ConnectionProperties>
      <rd:DataSourceID>e.g: vs generated numbers (f782cf55-155e-4beb-af11-b01072503bb5)</rd:DataSourceID>
    </DataSource>
  </DataSources>
  <DataSets>
    <DataSet Name="data (give a name here)"> ####### (ReportDataSource name) #######
      <Query>
        <DataSourceName>BrCaixaSeguradoraSCPJudObjetosTransferencia</DataSourceName>
        <CommandText>/* Local Query */</CommandText>
      </Query>
      <Fields>
        <Field Name="Class name field">
          <DataField>Class name field</DataField>
          <rd:TypeName>System.String</rd:TypeName>
        </Field>
      </Fields>
      <rd:DataSetInfo>
        <rd:DataSetName>full Namespace</rd:DataSetName>
        <rd:TableName>object class</rd:TableName>
        <rd:ObjectDataSourceType>fullnamespace.objectclassname, fullnamespace, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ZZZZXXXXZZZZ</rd:ObjectDataSourceType>
      </rd:DataSetInfo>
    </DataSet>
  </DataSets>
  <rd:ReportUnitType>Cm</rd:ReportUnitType>
  <rd:ReportID>e.g: vs generated numbers  (c33e36c2-216c-4d63-9c1d-b0cc8754c394)</rd:ReportID>
</Report>




public FileStreamResult Export(Filter filter)
{
  
  //This should be the same class used to set up the RDLC file!
	Collection<ClassName> list = null;  

	using (var obj = ServiceConsumerFactory.Create<IExample>("*"))
	{
		this.policyAttempt.ExecuteAction(() =>
		{
			list = obj.Operations.Get(filter);
		});
	}

	ReportViewer export = new ReportViewer();
	exportacao.ID = "export";
	exportacao.ProcessingMode = ProcessingMode.Local;
	exportacao.LocalReport.DataSources.Add(new ReportDataSource("data", list));
	exportacao.LocalReport.ReportPath = Server.MapPath(@"..\Content\export\NameReport.rdlc");

	Warning[] warnings;
	string[] streamids;
	string mimeType;
	string encoding;
	string extension;

	byte[] bytes = export.LocalReport.Render("Excel", "", out mimeType, out encoding, out extension, out streamids, out warnings);

	return new FileStreamResult(new MemoryStream(bytes), "application/ms-excel")
	{
		FileDownloadName = string.Concat("Report_", DateTime.Now.ToString("yyyy-M-d", CultureInfo.CurrentCulture), ".xls")
	};

}
