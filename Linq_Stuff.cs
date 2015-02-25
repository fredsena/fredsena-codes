

        public List<SomeViewModel> Listar(string status = "", string name = "", 
                                          int Id = 0)
        {
            using (var db = new Entities())
            {
                var result = (from d in db.Table1
                             join e in db.vTable2 on d.Id equals e.Ide                             
                             select new SomeViewModel
                             {
                                 IdeEmp = d.Id,
                                 Idc = d.Idc
                             });
                             
                //ativos, inativos, todos
                if (!String.IsNullOrEmpty(status))                
                    result = result.Where(r => r.Idc == status);                

                if (!String.IsNullOrEmpty(name))                
                    result = result.Where(r => r.NomEmp.Contains(name));

                if (Id != 0)
                    result = result.Where(r => r.Id == Id);

                return result.OrderBy(o => o.NamEmp).ToList();
            }
        }
