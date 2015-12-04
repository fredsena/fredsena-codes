using System;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Dapper.Contrib.Extensions;

namespace DataRepository
{
    public abstract class AbstractRepository<T> : IDisposable
    {
        protected abstract string TableName { get; }
        protected IDbConnection Connection
        {
            get { return new SqlConnection(ConfigurationManager.ConnectionStrings["bdcContext"].ConnectionString); }
        }

        public virtual T FindSingle(string query, dynamic param)
        {
            dynamic item = null;

            using (IDbConnection cn = Connection)
            {
                cn.Open();
                var result = cn.Query(query, (object)param).SingleOrDefault();

                if (result != null)
                {
                    item = Map(result);
                }
            }

            return item;
        }

        public virtual IEnumerable<T> FindAll(string query)
        {
            var items = new List<T>();

            using (IDbConnection cn = Connection)
            {
                cn.Open();
                var results = cn.Query(query).ToList();

                for (int i = 0; i < results.Count(); i++)
                {
                    items.Add(Map(results.ElementAt(i)));
                }
            }

            return items;
        }

        public abstract T Map(dynamic result);

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Mvc.Models
{
    public class AdmissaoModel
    {
        public int AdmissaoId { get; set; }        
        
        [Required(ErrorMessage="Informe o tipo de admissão")]
        [Display(Name = "Tipo de  Admissão")]
        public string TipoAdmissaoId { get; set; }

        [Required(ErrorMessage="Informe o tipo de Admissão")]
        [Display(Name="Descrição Admissão")]
        public string DescricaoAdmissao { get; set; }

        [Display(Name="Ativo")]
        public string AdmisaoAtivo { get; set; }

    }
}﻿using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;


namespace Mvc
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts//vendor.js", "~/Scripts//ie.js",
                      "~/Scripts//main.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/angular").Include(
                    "~/Scripts/angular.js",
                    "~/Scripts/angular-route.js", "~/App/app.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/vendor.css",
                      "~/Content/main.css"));
        }
    }
}
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc.Models
{
    public class CadastroFuncionarioDTO
    {
        public FuncionarioDTO funcionario { get; set; }
        public List<DependentesDTO> dependentes { get; set; }
        public List<DocumentosDTO> documentos { get; set; }


    }
}﻿
namespace Model.Empregado.Status
{
    public sealed class ContratoEstagiario: EmpregadoContrato
    {
        public ContratoEstagiario()
        {
            AdmissaoTipo = EmpregadoAdmissaoTipo.Nenhum;
        }

        public override EmpregadoContratoTipo ContratoTipo
        {
            get { return EmpregadoContratoTipo.Estagiario; }
        }

        public override EmpregadoAdmissaoTipo AdmissaoTipo
        {
            get { return EmpregadoAdmissaoTipo.Nenhum; }
            set { }
        }
    }
}
﻿
namespace Model.Empregado.Status
{
    public sealed class Contrato: EmpregadoContrato
    {
        public override EmpregadoContratoTipo ContratoTipo
        {
            get { return EmpregadoContratoTipo.Publico; }
        }
        public override EmpregadoAdmissaoTipo AdmissaoTipo { get; set; }
        
    }
}
﻿
namespace Model.Empregado.Status
{
    public sealed class ContratoTerceirizado: EmpregadoContrato
    {
        public ContratoTerceirizado()
        {
            AdmissaoTipo = EmpregadoAdmissaoTipo.Nenhum;
        }
        public override EmpregadoContratoTipo ContratoTipo
        {
            get { return EmpregadoContratoTipo.Terceirizado; }
        }
        public override EmpregadoAdmissaoTipo AdmissaoTipo
        {
            get { return EmpregadoAdmissaoTipo.Nenhum; }
            set { }
        }

    }
}
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mvc.Core;
using Mvc.Models;

namespace Mvc.Controllers
{
    public class CustomerController : Controller
    {
        // GET: Customer
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult EnviaMensagem()
        {
            AdmissaoModel admissao = new AdmissaoModel() { AdmisaoAtivo = "S" };
            Mensagem mensagem = new Mensagem
            {
                Text = "ERRO SISTEMA",
                TipoMensagem = TipoMensagem.Confirmacao
            };

            ViewData["Admissao"] = admissao;
            return Json(new { Admissao = admissao}, JsonRequestBehavior.AllowGet); 
        }

        // GET: Customer/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Customer/Create
        public ActionResult Create()
        {
            AdmissaoModel admissao = new AdmissaoModel() {AdmisaoAtivo = "S" };

            ViewBag.TipoAdmissaoId = new SelectList(Mvc.Util.Util.TipoAdmissao(), "key", "Value");

            return View(admissao);
        }

        // POST: Customer/Create
        [HttpPost]
        public ActionResult Create(CustomerModel customer)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Customer/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Customer/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Customer/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Customer/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc.Models
{
    public class CustomerModel
    {
        
        public int CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Mvc.Models;

namespace Mvc.Controllers
{
    [RoutePrefix("api")]

    public class DataApiController : ApiController
    {
        [HttpGet]
        [Route("customers")]
        public HttpResponseMessage GetCustomers(HttpRequestMessage request)
        {
            var customers = DataFactory.GetCustomers();

            return request.CreateResponse<CustomerModel[]>(HttpStatusCode.OK, customers.ToArray());
        }

        [HttpGet]
        [Route("customer/{customerId}")]
        public HttpResponseMessage GetCustomer(HttpRequestMessage request, int customerId)
        {
            var customers = DataFactory.GetCustomers();
            var customer = customers.FirstOrDefault(item => item.CustomerId == customerId);

            return request.CreateResponse<CustomerModel>(HttpStatusCode.OK, customer);
        }
    }
}
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc.Models
{
    public static class DataFactory
    {
        public static IEnumerable<CustomerModel> GetCustomers()
        {
            return new List<CustomerModel>()
            {
                new CustomerModel() { CustomerId = 1, FirstName = "Miguel", LastName = "Castro", Email = "miguelcastro67@gmail.com" },
                new CustomerModel() { CustomerId = 2, FirstName = "John", LastName = "Petersen", Email = "johnvpetersen@gmail.com" },
                new CustomerModel() { CustomerId = 3, FirstName = "Brian", LastName = "Noyes", Email = "briannoyes@gmail.com.br" },
                new CustomerModel() { CustomerId = 4, FirstName = "Andrew", LastName = "Brust", Email = "andrewbrust@gmail.com" },
                new CustomerModel() { CustomerId = 5, FirstName = "Rocky", LastName = "Lhotka", Email = "rockylhotka@gmail.com.br" }
            };
        }

        public static CadastroFuncionarioDTO GetFuncionario()
        {

            var func = new FuncionarioDTO() { FuncionarioId = 1, Nome = "Funcionario" };

            var dep = new List<DependentesDTO>() { 
                        new DependentesDTO(){ DependenteId = 1 , DescDependente = "Filho1" , FuncionarioId = 1 }, 
                        new DependentesDTO(){ DependenteId = 2 , DescDependente = "Filho2" , FuncionarioId = 1 },
                        new DependentesDTO(){ DependenteId = 3 , DescDependente = "Esposa" , FuncionarioId = 1 },
                        new DependentesDTO(){ DependenteId = 4 , DescDependente = "Enteado" , FuncionarioId = 1 }
            };

            var doc = new List<DocumentosDTO>() 
            { 
                        new DocumentosDTO() {DocumentoId = 1, DescDocDoc = "Certidao Nascimento" , FuncionarioId = 1},
                        new DocumentosDTO() {DocumentoId = 1, DescDocDoc = "Certidao Casamento" ,FuncionarioId = 1},
                        new DocumentosDTO() {DocumentoId = 1, DescDocDoc = "Título de Eleitor" ,FuncionarioId = 1},
                        new DocumentosDTO() {DocumentoId = 1, DescDocDoc = "Passaporte" ,FuncionarioId = 1},
                        new DocumentosDTO() {DocumentoId = 1, DescDocDoc = "Certificação X" ,FuncionarioId = 1},
                        new DocumentosDTO() {DocumentoId = 1, DescDocDoc = "Diploma Curso Superior" ,FuncionarioId = 1}
            };

            CadastroFuncionarioDTO cad = new CadastroFuncionarioDTO
            {
                funcionario = func,
                dependentes = dep,
                documentos = doc
            };

            return cad;
        }
    }
}﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc.Models
{
    public class DependentesDTO

    {
        public int DependenteId { get; set; }

        public string DescDependente { get; set; }

        public int FuncionarioId { get; set; }

    }
}﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc.Models
{
    public class DocumentosDTO
    {
        public int DocumentoId { get; set; }
        public string DescDocDoc  { get; set; }
        public int FuncionarioId { get; set; }
    }
}﻿using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Infrastructure.Domain.Events
{
    /// <summary>
    /// The domain events container for registering domain event callbacks.
    /// </summary>
    /// <remarks>This implementation is based on the code example from this blog:
    /// http://www.udidahan.com/2009/06/14/domain-events-salvation/
    /// </remarks>
    public static class DomainEvents
    {
        /// <summary>
        /// The _actions.
        /// </summary>
        /// <remarks>Marked as ThreadStatic that each thread has its own callbacks</remarks>
        [ThreadStatic]
        private static List<Delegate> _actions;

        /// <summary>
        /// The container
        /// </summary>
        public static IEventContainer Container;

        /// <summary>
        /// Registers the specified callback for the given domain event.
        /// </summary>
        /// <typeparam name="T">
        /// </typeparam>
        /// <param name="callback">
        /// The callback.
        /// </param>
        public static void Register<T>(Action<T> callback) where T : IDomainEvent
        {
            if (_actions == null)
                _actions = new List<Delegate>();

            _actions.Add(callback);
        }

        /// <summary>
        /// Raises the specified domain event and calls the event handlers.
        /// </summary>
        /// <typeparam name="T">
        /// </typeparam>
        /// <param name="domainEvent">
        /// The domain event.
        /// </param>
        public static void Raise<T>(T domainEvent) where T : IDomainEvent
        {
            if (Container != null)
                foreach (var handler in Container.Handlers(domainEvent))
                    handler.Handle(domainEvent);

            // registered actions, typically used for unit tests.
            if (_actions != null)
                foreach (var action in _actions)
                    if (action is Action<T>)
                        ((Action<T>)action)(domainEvent);
        }
    }
}
﻿
using System;
using Infrastructure.Domain;
using Model.Empregado.Status;
using Dapper.Contrib.Extensions;

namespace Model.Empregado
{
    [Table("dbo.Empregado")] 
    public abstract class Empregado : EntityBase, IAggregateRoot
    {
        [Key]       
        public int Ide { get; set; }        

        [Computed]
        public string StatusEmpregado 
        {
            get { return Enum.GetName(typeof(EmpregadoStatusTipo), TipoStatus.Status); } 
        }

        [Computed]
        public string Contrato { get; set; }

        [Computed]
        public string Lotacao { get; set; }

        [Computed]
        public string Cargo { get; set; }

        [Computed]
        public IEmpregadoStatus TipoStatus { get; private set; }

        [Computed]
        public IEmpregadoContrato TipoContratoStatus { get; set;}

        [Computed]
        public EmpregadoStatusTipo Status 
        { 
            get { return TipoStatus.Status; } 
        }

        public abstract void SetTipoContratoStatus(IEmpregadoContrato ContratoStatus);
        
        public void SetStatus(IEmpregadoStatus status)
        {
            this.TipoStatus = status;
        }

        /*
        public void Notifica()
        {
            this.TipoStatus.Notifica(this);
        }
        */

        public override void Validate()
        {
            if (string.IsNullOrWhiteSpace(NomEmp)) ValidationErrors.Add("informe o Nome");

            if (this.TipoContratoStatus.ContratoTipo == EmpregadoContratoTipo.Nenhum) ValidationErrors.Add("Tipo Contrato inválido");

            if (this.Status == EmpregadoStatusTipo.Indefinido) ValidationErrors.Add("Tipo status inválido: Indefinido");
        }
    }
}
﻿using System;
using System.Collections.Generic;
using Model.Empregado;
using Model.Services;

namespace Services
{
    public class EmpregadoAppService: IEmpregadoAppService
    {
        private readonly IEmpregadoService _empregadoService;

        public EmpregadoAppService(IEmpregadoService empregadoService)
        {
            this._empregadoService = empregadoService;
        }

        public Empregado FindById(int id)
        {
            return _empregadoService.FindById(id);
        }

        public IEnumerable<Empregado> FindAll()
        {
            return _empregadoService.FindAll();
        }

        public void Insert(Empregado item)
        {
            throw new NotImplementedException();
        }

        public void Update(Empregado item)
        {
            throw new NotImplementedException();
        }
    }
}
﻿
namespace Model.Empregado.Status
{
    public abstract class EmpregadoContrato : IEmpregadoContrato
    {
        public abstract EmpregadoContratoTipo ContratoTipo { get; }

        public abstract EmpregadoAdmissaoTipo AdmissaoTipo { get; set; }
    }
}
﻿
namespace Model.Empregado.Status
{
    public enum EmpregadoContratoTipo : int
    {
        Nenhum = 0,
        Publico = 1,
        Terceirizado = 14,
        Estagiario = 59
    }
}
﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.Empregado;
using Services;

namespace Mvc.Controllers
{
    public class EmpregadoController : Controller
    {
        private readonly IEmpregadoAppService service;

        public EmpregadoController(IEmpregadoAppService _service)
        {
            this.service = _service;
        }       

        // GET: Empregado
        public ActionResult Index()
        {
            return View(service.FindAll());
        }

        // GET: Empregado/Details/5
        public ActionResult Details(int id)
        {
            var empregado = service.FindById(id);

            return View();
        }

        // GET: Empregado/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Empregado/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Empregado/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Empregado/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Empregado/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Empregado/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
﻿
using Model.Empregado.Status;
using Dapper.Contrib.Extensions;

namespace Model.Empregado
{
    [Table("dbo.Empregado")] 
    public class EmpregadoEstagiario : Empregado, IEmpregado
    {
        public EmpregadoEstagiario()
        {
            this.IdeEmp = 0;
            this.TipoContratoStatus = new ContratoEstagiario();
            this.SetStatus(new StatusIndefinido());
        }
        public override void SetTipoContratoStatus(IEmpregadoContrato ContratoStatus)
        {
            this.TipoContratoStatus = new ContratoEstagiario();
        }

        public override void Validate()
        {
            base.Validate();
        }
    }
}
﻿
using System;
using System.Collections.Generic;
using System.Reflection;
using Model.Empregado.Status;
using Model.Empregado.Helpers;

namespace Model.Empregado
{    
    public class EmpregadoFactory : IEmpregadoFactory
    {
        Dictionary<string, Type> empregados;

        public EmpregadoFactory()
        {
            LoadTypesICanReturn();
        }

        public IEmpregado CreateInstance(IEmpregadoContrato contrato)
        {
            Type t = GetTypeToCreate(contrato, empregados);
            Empregado emp = Activator.CreateInstance(t) as Empregado;
            emp.TipoContratoStatus = contrato;
            return emp as IEmpregado;
        }

        private Type GetTypeToCreate(IEmpregadoContrato Contrato, Dictionary<string, Type> empregados)
        {
            foreach (var empregado in empregados)
            {
                Type tipoObjeto = Type.GetType(empregado.Value.ToString());
                ConstructorInfo objEmpregado = tipoObjeto.GetConstructor(Type.EmptyTypes);
                object ClassObjEmpregado = objEmpregado.Invoke(new object[] { });

                IEmpregadoContrato _contrato = (IEmpregadoContrato)ClassObjEmpregado.GetType().GetProperty("TipoContratoStatus").GetValue(ClassObjEmpregado, null);

                if (_contrato.ContratoTipo == Contrato.ContratoTipo)
                {
                    return empregados[empregado.Key];
                }
            }

            return null;
        }

        private void LoadTypesICanReturn()
        {
            empregados = new Dictionary<string, Type>();

            Type[] typesInThisAssembly = Assembly.GetExecutingAssembly().GetTypes();

            foreach (Type type in typesInThisAssembly)
            {
                if (type.GetInterface(typeof(IEmpregado).ToString()) != null)
                {
                    empregados.Add(type.Name.ToLower(), type);
                }
            }
        }
    }
}
﻿
using Model.Empregado.Status;

namespace Model.Empregado.Helpers
{
    public static class EmpregadoHelper
    {
        public static IEmpregadoContrato ConverteTipoContratoFromBanco(int AtividadeId)
        {
            switch (AtividadeId)
            {
                case 1://VERIFICAR
                case 60: //VERIFICAR
                case 2:
                case 5:
                case 57:
                case 58:
                case 62:
                case 63:
                    return new Contrato() { AdmissaoTipo = (EmpregadoAdmissaoTipo) AtividadeId };                    
                case 14:
                    return new ContratoTerceirizado();
                case 59:
                    return new ContratoEstagiario();
            }
            
            return null;
        }
    }
}
﻿using System;
using System.Collections.Generic;
using Model.Empregado;
using Model.Empregado.Helpers;
using Model.Empregado.Status;
using System.Data;
using Dapper;
using Dapper.Contrib.Extensions;

namespace DataRepository
{
    public class EmpregadoRepository : AbstractRepository<Empregado>, IEmpregadoRepository
    {
        protected override string TableName
        {
            get { return "dbo.Empregado"; }
        }

        public Empregado FindByContratoTipo(string ContratoId)
        {
            return base.FindSingle("SELECT * FROM dbo.Empregado WHERE AtvTc=@ContratoId", new { ContratoId = ContratoId });
        }

        public IEnumerable<Empregado> FindAll()
        {
            string query = @"
            SELECT DISTINCT ";

            return base.FindAll(query);
        }

        public Empregado FindById(int IdeEmp)
        {
            string query = @"
            SELECT DISTINCT ";

            return base.FindSingle(query, new { IdeEmp = IdeEmp });
        }

        public int Insert(Empregado empregado)
        {
            using (IDbConnection cn = base.Connection)
            {
                return (int) cn.Insert<Empregado>(empregado);
            }
        }

        public void Update(Empregado empregado)
        {
            using (IDbConnection cn = base.Connection)
            {
                cn.Update<Empregado>(empregado);
            }
        }

        public override Empregado Map(dynamic result)
        {
            Empregado empregado;

            empregado = new EmpregadoFactory().CreateInstance(EmpregadoHelper.ConverteTipoContratoFromBanco(result.AtividadeId));

            string DatFimAds = Convert.ToString(result.DatFimAds);

            empregado.SetStatus((string.IsNullOrEmpty(DatFimAds)) ? (IEmpregadoStatus)new StatusAtivo() : (IEmpregadoStatus)new StatusInativo());

            if (empregado.TipoContratoStatus.ContratoTipo == EmpregadoContratoTipo.Publico)            
                empregado.Contrato = Enum.GetName(typeof(EmpregadoAdmissaoTipo), empregado.TipoContratoStatus.AdmissaoTipo);            
            else
                empregado.Contrato = Enum.GetName(typeof(EmpregadoContratoTipo), empregado.TipoContratoStatus.ContratoTipo);


            empregado.Lotacao = result.Lotacao;
            empregado.Cargo = result.Cargo;

            empregado.Ide = result.IdeEmp;

            return empregado;
        }
    }
}
﻿
using Model.Empregado;
using Model.Services;
using System.Collections.Generic;

namespace DataRepository
{
    public class EmpregadoService : IEmpregadoService
    {

        private readonly IEmpregadoRepository empregadoRepository;

        public EmpregadoService(IEmpregadoRepository _empregadoRepository)
        {
            empregadoRepository = _empregadoRepository;
        }

        public IEnumerable<Empregado> FindAll()
        {
            return empregadoRepository.FindAll();
        }

        public Empregado FindById(int id)
        {
            return empregadoRepository.FindById(id);
        }

        public void Insert(Empregado item)
        {
            throw new System.NotImplementedException();
        }

        public void Update(Empregado item)
        {
            throw new System.NotImplementedException();
        }
    }
}
﻿
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoRhinoMock;
using Ploeh.AutoFixture.DataAnnotations;

using DataRepository;
using Model.Empregado;
using Services;
using Model.Empregado.Status;
using System.Reflection;
using System.Collections.Concurrent;

using Dapper;
using Dapper.Contrib.Extensions;


namespace Model.Tests.Empregado
{
    public class EmpregadoServiceTests
    {
        private Fixture fixture;


        [Fact]
        public void Find_EmpregadoById()
        {
            IEmpregadoRepository repository = new EmpregadoRepository();
            Model.Empregado.Empregado emp = repository.FindById(2);

            Assert.True(emp.GetType() == typeof(EmpregadoTerceirizado));

            Assert.True(emp.TipoContratoStatus.AdmissaoTipo == EmpregadoAdmissaoTipo.Nenhum);
        }

        [Fact]
        //TODO: Rever a rotina de busca de ATIVOS INATIVOS
        public void SetEmpregadoAtivoInativo()
        {
            IEmpregadoRepository repository = new EmpregadoRepository();
            Model.Empregado.Empregado emp = repository.FindById(2);
            Assert.False(emp.Status == EmpregadoStatusTipo.Indefinido);
        }

        [Fact]
        public void UpdateEmpregado()
        {
            IEmpregadoRepository repository = new EmpregadoRepository();

            Model.Empregado.Empregado emp = repository.FindById(2);

            Assert.Equal(emp.NomEmp.Trim(), "F");

            emp.NomEmp = "TESTE 123";
            repository.Update(emp);

            emp = repository.FindById(2);
            Assert.Equal(emp.NomEmp.Trim(), "TESTE 123");

            emp.NomEmp = "F";
            repository.Update(emp);
        }

        [Fact]
        public void InsertEmpregado()
        {
            IEmpregadoRepository repository = new EmpregadoRepository();

            int valorId = repository.Insert(new Empregado
            {
                NomEmp = "TESTE 123",
                MatEmp = "",
                RgEmp = "123",
                OrgExpRgEmp = "ORG",
                SigEtdRgEmp = "S",
                SexEmp = "M"
            });            

            using (IDbConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["bdcContext"].ToString()))
            {
                var emp = Dapper.Contrib.Extensions.SqlMapperExtensions.Get<Empregado>(conn, valorId);

                Assert.NotNull(emp);

                Dapper.Contrib.Extensions.SqlMapperExtensions.Delete<Empregado>(conn, emp);
            }
        }

        #region ###### Find Attributes Test ######
        static ConcurrentDictionary<Type, List<string>> paramNameCache = new ConcurrentDictionary<Type, List<string>>();
        internal static List<string> GetParamNames(object o)
        {
            if (o is DynamicParameters)
            {
                return (o as DynamicParameters).ParameterNames.ToList();
            }

            List<string> paramNames;
            if (!paramNameCache.TryGetValue(o.GetType(), out paramNames))
            {
                paramNames = new List<string>();
                foreach (var prop in o.GetType().GetProperties(BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.Public))
                {
                    var attribs = prop.GetCustomAttributes(true);
                    var attr = attribs.FirstOrDefault() as ComputedAttribute;

                    //if (attr == null || (attr != null && !attr.Value))
                    if (attr != null)
                    {
                        paramNames.Add(prop.Name);
                    }
                }
                paramNameCache[o.GetType()] = paramNames;
            }
            return paramNames;
        }

        [Fact]
        public void Identifica_Atributo_Dapper_ComputedAttribute()
        {
            Empregado emp = new Empregado();

            List<string> paramNames = GetParamNames(emp);

            Assert.Contains("TipoStatus", paramNames);
        }

        #endregion


    }
}
﻿
using System;
using Infrastructure.Domain;
using Model.Empregado.Status;
using Dapper.Contrib.Extensions;

namespace Model.Empregado
{
    [Table("dbo.Empregado")] 
    public class Empregado : Empregado, IEmpregado
    {
        public DateTime DataAlteracao { get; set; }

        public Empregado()
        {
            this.IdeEmp = 0;
            this.DataAlteracao = DateTime.Now;
            this.TipoContratoStatus = new Contrato();
            this.SetStatus(new StatusIndefinido());
        }

        public override void SetTipoContratoStatus(IEmpregadoContrato ContratoStatus)
        {
            this.TipoContratoStatus = new Contrato();
        }

        public override void Validate()
        {
            base.Validate();

            if (this.TipoContratoStatus.AdmissaoTipo == EmpregadoAdmissaoTipo.Nenhum) ValidationErrors.Add("Tipo Admissão inválida");            
        }
    }
}
﻿
namespace Model.Empregado.Status
{
    public enum EmpregadoAdmissaoTipo: int
    {
        Nenhum = 0,       
        Temporário = 1//VERIFICAR
        
    }
}
﻿
namespace Model.Empregado.Status
{
    public abstract class EmpregadoStatus : IEmpregadoStatus
    {
        public abstract EmpregadoStatusTipo Status { get; }
        public abstract void Notifica(Empregado empregado);        
    }
}
﻿
namespace Model.Empregado.Status
{
    public enum EmpregadoStatusTipo: int
    {
        Indefinido = 0,
        Ativo = 1,
        Inativo = 2
    }
}
﻿
using System;
using Model.Empregado.Status;
using Dapper.Contrib.Extensions;

namespace Model.Empregado
{
    [Table("dbo.Empregado")] 
    public class EmpregadoTerceirizado : Empregado, IEmpregado
    {
        public EmpregadoTerceirizado()
        {
            this.IdeEmp = 0;
            this.TipoContratoStatus = new ContratoTerceirizado();
            this.SetStatus(new StatusIndefinido());
        }

        public override void SetTipoContratoStatus(IEmpregadoContrato ContratoStatus)
        {
            this.TipoContratoStatus = new ContratoTerceirizado();
        }

        public override void Validate()
        {
            base.Validate();
        }
    }
}
﻿using System;
using Xunit;
using Model.Empregado;
using Model.Empregado.Status;
using Model.Empregado.Events;
using Infrastructure.Domain.Events;
using Services;

using System.Reflection;


namespace Model.Tests.Empregado
{
    public class EmpregadoTests
    {
        public EmpregadoTests()
        {
        }

        //[Fact]
        //public void Get_Name_Enum()
        //{
        //    string teste = Enum.GetName(typeof(EmpregadoAdmissaoTipo),63);
        //}


        [Fact]
        public void Get_EnumValue_EmpregadoAdmissaoTipo_EFETIVO()
        {
            EmpregadoAdmissaoTipo foo = (EmpregadoAdmissaoTipo)63;
            Assert.True(EmpregadoAdmissaoTipo.EFETIVO == foo);
        }

        [Fact]
        public void EmpregadoInvalido()
        {
            Model.Empregado.Empregado emp = new EmpregadoEstagiario();            
            Assert.False(emp.IsValid);

            emp = new Empregado();
            Assert.False(emp.IsValid);

            emp = new EmpregadoTerceirizado();
            Assert.False(emp.IsValid);
        }

        [Fact]
        public void Empregado_Valido()
        {
            Model.Empregado.Empregado emp = new Empregado();

            emp.NomEmp = "USUARIO TESTE ";
            emp.TipoContratoStatus = new Contrato() {AdmissaoTipo = EmpregadoAdmissaoTipo.EFETIVO };
            emp.SetStatus(new StatusAtivo());
            Assert.True(emp.IsValid);
        }

        [Fact]
        public void Empregado_TerceirizadoValido()
        {
            Model.Empregado.Empregado emp = new EmpregadoTerceirizado();
            emp.NomEmp = "USUARIO TESTE TERCEIRIZADO";
            emp.SetStatus(new StatusInativo());
            Assert.True(emp.IsValid);
        }

        [Fact]
        public void InativaEmpregado()
        {
            Model.Empregado.Empregado emp = new Empregado();
            emp.NomEmp = "USUARIO TESTE ";

            emp.SetStatus(new StatusInativo());

            var statusEmp = new StatusInativo();

            InativaEmpregadoEvent inativaEvent = null;

            DomainEvents.Register<InativaEmpregadoEvent>(evt => inativaEvent = evt);

            statusEmp.Notifica(emp);

            Assert.Equal(EmpregadoStatusTipo.Inativo, emp.Status);
            
        }

        [Fact]
        public void GetEmpregado__FromFactory()
        {
            IEmpregado Emp = new EmpregadoFactory().CreateInstance(new Contrato());
            Assert.True(Emp.GetType() == typeof(Empregado));
        }

        [Fact]
        public void GetEmpregado_Estagiario_FromFactory()
        {
            IEmpregado EmpEstag = new EmpregadoFactory().CreateInstance(new ContratoEstagiario());
            Assert.True(EmpEstag.GetType() == typeof(EmpregadoEstagiario));
        }

        [Fact]
        public void GetEmpregado_Terceirizado_FromFactory()
        {
            IEmpregado EmpTerceirizado = new EmpregadoFactory().CreateInstance(new ContratoTerceirizado());
            Assert.True(EmpTerceirizado.GetType() == typeof(EmpregadoTerceirizado));
        }
    }
}
﻿
using Dapper.Contrib.Extensions;

namespace Infrastructure.Domain
{
    public abstract class EntityBase : IValidate
    {
        private readonly ValidationErrors _validationErrors;

        public EntityBase()
        {
            _validationErrors = new ValidationErrors();
        }

        [Computed]
        public virtual bool IsValid
        {
            get 
            {
                _validationErrors.Clear();
                Validate();
                return ValidationErrors.Items.Count == 0;
            }
        }

        [Computed]
        public ValidationErrors ValidationErrors
        {
            get { return _validationErrors; }
        }

        public abstract void Validate();
  
    }
}
﻿
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc.Models
{
    public class FuncionarioDTO
    {
        public int FuncionarioId { get; set; }
        public string Nome { get; set; }

    }
}﻿using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Mvc
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public static object globalErroCritical = null;
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            ModelBinders.Binders.DefaultBinder = new CustomModelBinder();

            globalStart.SystemConfigStartup(this);

            IoC.InitIoc();
        }
    }
}
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mvc.Models;
using .Core.Filters;

namespace Mvc.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var func = View(DataFactory.GetFuncionario());
            return func;
        }

        [AuthorizationCore]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult _FuncionarioTab()
        {
            var Funcionario = new FuncionarioDTO() { FuncionarioId = 1, Nome = "Sena" };
            return PartialView(Funcionario);
        }

        public ActionResult _DocumentosTab(List<DocumentosDTO> Documento)
        {

            return PartialView(Documento);
        }

        public ActionResult _DependentesTab(List<DependentesDTO> Dependente)
        {

            return PartialView(Dependente);
        }


    }


}﻿using System;

namespace Infrastructure.Domain
{
    /// <summary>
    /// The aggregate root for use in the repository.
    /// </summary>
    /// <remarks>
    /// This indicates what objects can be directly loaded from the repository.
    /// </remarks>
    public interface IAggregateRoot
    {
        
    }
}
﻿using System.Collections.Generic;

namespace Services
{
    public interface IAppService<T> where T : class 
    {
        T FindById(int id);
        IEnumerable<T> FindAll();
        void Insert(T item);
        void Update(T item); 
    }
}
﻿
namespace Infrastructure.Domain.Events
{
    /// <summary>
    /// Interface for explicitly representing domain events.
    /// </summary>
    public interface IDomainEvent
    {
    }
}
﻿
namespace Infrastructure.Domain.Events
{
    /// <summary>
    /// Domain event handler interface.
    /// </summary>
    /// <typeparam name="T">
    /// </typeparam>
    public interface IDomainEventHandler<T> where T : IDomainEvent
    {
        /// <summary>
        /// Handles the specified domain event.
        /// </summary>
        /// <param name="domainEvent">
        /// The domain event.
        /// </param>
        void Handle(T domainEvent);
    }
}
﻿
namespace Model.Empregado
{
    public interface IEmpregado
    {
        
    }
}
﻿
namespace Services
{
    public interface IEmpregadoAppService : IAppService<Model.Empregado.Empregado>
    {

    }
}
﻿
namespace Model.Empregado.Status
{
    public interface IEmpregadoContrato
    {
        EmpregadoContratoTipo ContratoTipo { get; }
        EmpregadoAdmissaoTipo AdmissaoTipo { get; set; }
    }
}
﻿using Model.Empregado.Status;

namespace Model.Empregado
{
    public interface IEmpregadoFactory
    {
        IEmpregado CreateInstance(IEmpregadoContrato ContratoTipo);
    }
}
﻿
using Infrastructure.Domain;
using System.Collections.Generic;

namespace Model.Empregado
{
    public interface IEmpregadoRepository : IRepository<Empregado>
    {
        Empregado FindByContratoTipo(string ContratoId);
    }
}
﻿using Infrastructure.Services;
using System.Collections.Generic;

namespace Model.Services
{
    public interface IEmpregadoService : IService<Model.Empregado.Empregado>
    {

    }
}
﻿
namespace Model.Empregado.Status
{
    public interface IEmpregadoStatus
    {
        EmpregadoStatusTipo Status { get; }

        void Notifica(Empregado empregado);
    }
}
﻿using System.Collections.Generic;
namespace Infrastructure.Domain.Events
{
    /// <summary>
    /// The EventContainer interface.
    /// </summary>
    public interface IEventContainer
    {
        IEnumerable<IDomainEventHandler<T>> Handlers<T>(T domainEvent) where T : IDomainEvent;
    }
}
﻿
using Infrastructure.Domain.Events;

namespace Model.Empregado.Events
{
    public class InativaEmpregadoEvent: IDomainEvent
    {
        public InativaEmpregadoEvent(Empregado empregado)
        {
            this.Empregado = empregado;
            //TODO: Envia Email
        }

        public Empregado Empregado { get; set; }
    }
}
﻿
using System.Web.Mvc;
using SimpleInjector;
using SimpleInjector.Integration.Web;
using SimpleInjector.Integration.Web.Mvc;

using DataRepository;
using Model.Empregado;
using Services;
using Model.Services;

namespace Mvc
{
    public class IoC
    {
        public static void InitIoc()
        {
            var container = new Container();
            container.Options.DefaultScopedLifestyle = new WebRequestLifestyle();

            container.Register<IEmpregadoService, EmpregadoService>(Lifestyle.Scoped);
            container.Register<IEmpregadoRepository, EmpregadoRepository>(Lifestyle.Scoped);
            container.Register<IEmpregadoAppService, EmpregadoAppService>(Lifestyle.Scoped);

            container.RegisterMvcControllers(System.Reflection.Assembly.GetExecutingAssembly());
            container.RegisterMvcIntegratedFilterProvider();
            container.Verify();
            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));
        }
    }
}﻿using System.Collections.Generic;

namespace Infrastructure.Domain
{
    public interface IRepository<T> where T : EntityBase
    {
        T FindById(int id);
        IEnumerable<T> FindAll();
        int Insert(T item);
        void Update(T item);        
    }
}
﻿using System.Collections.Generic;

namespace Infrastructure.Services
{
    public interface IService<T> where T :class 
    {
        T FindById(int id);
        IEnumerable<T> FindAll();
        void Insert(T item);
        void Update(T item); 
    }
}
﻿
namespace Infrastructure.Domain
{
    public interface IValidate
    {
        bool IsValid { get; }

        ValidationErrors ValidationErrors { get; }
    }
}
﻿
using System;

namespace Mvc.Core
{
    public class Mensagem
    {
        public string Text { get; set; }
        public TipoMensagem TipoMensagem { get; set; }

    }
}﻿

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Mvc
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            /*
            routes.MapRoute(
                name:"Customer",
                url: "Customer/{*catchall}",
                defaults: new { controller = "Customer", action = "Index", catchall = UrlParameter.Optional });
            */
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Collections.Concurrent;
using System.Reflection.Emit;
using System.Threading;
using Dapper;
using Dapper.Contrib.Extensions;

namespace DataRepository
{
    public static class SqlMapperExtensions
    {

        // ReSharper disable once MemberCanBePrivate.Global
        public interface IProxy //must be kept public
        {
            bool IsDirty { get; set; }
        }

        public interface ITableNameMapper
        {
            string GetTableName(Type type);
        }

        public delegate string GetDatabaseTypeDelegate(IDbConnection connection);
        public delegate string TableNameMapperDelegate(Type type);

        private static readonly ConcurrentDictionary<RuntimeTypeHandle, IEnumerable<PropertyInfo>> KeyProperties = new ConcurrentDictionary<RuntimeTypeHandle, IEnumerable<PropertyInfo>>();
        private static readonly ConcurrentDictionary<RuntimeTypeHandle, IEnumerable<PropertyInfo>> ExplicitKeyProperties = new ConcurrentDictionary<RuntimeTypeHandle, IEnumerable<PropertyInfo>>();
        private static readonly ConcurrentDictionary<RuntimeTypeHandle, IEnumerable<PropertyInfo>> TypeProperties = new ConcurrentDictionary<RuntimeTypeHandle, IEnumerable<PropertyInfo>>();
        private static readonly ConcurrentDictionary<RuntimeTypeHandle, IEnumerable<PropertyInfo>> ComputedProperties = new ConcurrentDictionary<RuntimeTypeHandle, IEnumerable<PropertyInfo>>();
        private static readonly ConcurrentDictionary<RuntimeTypeHandle, string> GetQueries = new ConcurrentDictionary<RuntimeTypeHandle, string>();
        private static readonly ConcurrentDictionary<RuntimeTypeHandle, string> TypeTableName = new ConcurrentDictionary<RuntimeTypeHandle, string>();

        private static readonly Dictionary<string, ISqlAdapter> AdapterDictionary = new Dictionary<string, ISqlAdapter> {
																							{"sqlconnection", new SqlServerAdapter()},
																							{"sqlceconnection", new SqlCeServerAdapter()},
																							{"npgsqlconnection", new PostgresAdapter()},
																							{"sqliteconnection", new SQLiteAdapter()},
																							{"mysqlconnection", new MySqlAdapter()},
																						};

        private static List<PropertyInfo> ComputedPropertiesCache(Type type)
        {
            IEnumerable<PropertyInfo> pi;
            if (ComputedProperties.TryGetValue(type.TypeHandle, out pi))
            {
                return pi.ToList();
            }

            var computedProperties = TypePropertiesCache(type).Where(p => p.GetCustomAttributes(true).Any(a => a is ComputedAttribute)).ToList();

            ComputedProperties[type.TypeHandle] = computedProperties;
            return computedProperties;
        }

        private static List<PropertyInfo> ExplicitKeyPropertiesCache(Type type)
        {
            IEnumerable<PropertyInfo> pi;
            if (ExplicitKeyProperties.TryGetValue(type.TypeHandle, out pi))
            {
                return pi.ToList();
            }

            var explicitKeyProperties = TypePropertiesCache(type).Where(p => p.GetCustomAttributes(true).Any(a => a is ExplicitKeyAttribute)).ToList();

            ExplicitKeyProperties[type.TypeHandle] = explicitKeyProperties;
            return explicitKeyProperties;
        }

        private static List<PropertyInfo> KeyPropertiesCache(Type type)
        {

            IEnumerable<PropertyInfo> pi;
            if (KeyProperties.TryGetValue(type.TypeHandle, out pi))
            {
                return pi.ToList();
            }

            var allProperties = TypePropertiesCache(type);
            var keyProperties = allProperties.Where(p => p.GetCustomAttributes(true).Any(a => a is KeyAttribute)).ToList();

            if (keyProperties.Count == 0)
            {
                var idProp = allProperties.FirstOrDefault(p => p.Name.ToLower() == "id");
                if (idProp != null)
                {
                    keyProperties.Add(idProp);
                }
            }

            KeyProperties[type.TypeHandle] = keyProperties;
            return keyProperties;
        }

        private static List<PropertyInfo> TypePropertiesCache(Type type)
        {
            IEnumerable<PropertyInfo> pis;
            if (TypeProperties.TryGetValue(type.TypeHandle, out pis))
            {
                return pis.ToList();
            }

            var properties = type.GetProperties().Where(IsWriteable).ToArray();
            TypeProperties[type.TypeHandle] = properties;
            return properties.ToList();
        }

        private static bool IsWriteable(PropertyInfo pi)
        {
            var attributes = pi.GetCustomAttributes(typeof(WriteAttribute), false);
            if (attributes.Length != 1) return true;

            var writeAttribute = (WriteAttribute)attributes[0];
            return writeAttribute.Write;
        }

        /// <summary>
        /// Returns a single entity by a single id from table "Ts".  
        /// Id must be marked with [Key] attribute.
        /// Entities created from interfaces are tracked/intercepted for changes and used by the Update() extension
        /// for optimal performance. 
        /// </summary>
        /// <typeparam name="T">Interface or type to create and populate</typeparam>
        /// <param name="connection">Open SqlConnection</param>
        /// <param name="id">Id of the entity to get, must be marked with [Key] attribute</param>
        /// <returns>Entity of T</returns>
        public static T Get<T>(this IDbConnection connection, dynamic id, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
        {
            var type = typeof(T);

            string sql;
            if (!GetQueries.TryGetValue(type.TypeHandle, out sql))
            {
                var keys = KeyPropertiesCache(type);
                var explicitKeys = ExplicitKeyPropertiesCache(type);
                if (keys.Count() > 1 || explicitKeys.Count() > 1)
                    throw new DataException("Get<T> only supports an entity with a single [Key] or [ExplicitKey] property");
                if (!keys.Any() && !explicitKeys.Any())
                    throw new DataException("Get<T> only supports an entity with a [Key] or an [ExplicitKey] property");

                var key = keys.Any() ? keys.First() : explicitKeys.First();

                var name = GetTableName(type);

                // TODO: query information schema and only select fields that are both in information schema and underlying class / interface 
                sql = "select * from " + name + " where " + key.Name + " = @id";
                GetQueries[type.TypeHandle] = sql;
            }

            var dynParms = new DynamicParameters();
            dynParms.Add("@id", id);

            T obj;

            if (type.IsInterface)
            {
                var res = connection.Query(sql, dynParms).FirstOrDefault() as IDictionary<string, object>;

                if (res == null)
                    return null;

                obj = ProxyGenerator.GetInterfaceProxy<T>();

                foreach (var property in TypePropertiesCache(type))
                {
                    var val = res[property.Name];
                    property.SetValue(obj, Convert.ChangeType(val, property.PropertyType), null);
                }

                ((IProxy)obj).IsDirty = false;   //reset change tracking and return
            }
            else
            {
                obj = connection.Query<T>(sql, dynParms, transaction, commandTimeout: commandTimeout).FirstOrDefault();
            }
            return obj;
        }

        /// <summary>
        /// Returns a list of entites from table "Ts".  
        /// Id of T must be marked with [Key] attribute.
        /// Entities created from interfaces are tracked/intercepted for changes and used by the Update() extension
        /// for optimal performance. 
        /// </summary>
        /// <typeparam name="T">Interface or type to create and populate</typeparam>
        /// <param name="connection">Open SqlConnection</param>
        /// <returns>Entity of T</returns>
        public static IEnumerable<T> GetAll<T>(this IDbConnection connection, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
        {
            var type = typeof(T);
            var cacheType = typeof(List<T>);

            string sql;
            if (!GetQueries.TryGetValue(cacheType.TypeHandle, out sql))
            {
                var keys = KeyPropertiesCache(type);
                if (keys.Count() > 1)
                    throw new DataException("Get<T> only supports an entity with a single [Key] property");
                if (!keys.Any())
                    throw new DataException("Get<T> only supports en entity with a [Key] property");

                var name = GetTableName(type);

                // TODO: query information schema and only select fields that are both in information schema and underlying class / interface 
                sql = "select * from " + name;
                GetQueries[cacheType.TypeHandle] = sql;
            }


            if (!type.IsInterface) return connection.Query<T>(sql, null, transaction, commandTimeout: commandTimeout);

            var result = connection.Query(sql);
            var list = new List<T>();
            foreach (IDictionary<string, object> res in result)
            {
                var obj = ProxyGenerator.GetInterfaceProxy<T>();
                foreach (var property in TypePropertiesCache(type))
                {
                    var val = res[property.Name];
                    property.SetValue(obj, Convert.ChangeType(val, property.PropertyType), null);
                }
                ((IProxy)obj).IsDirty = false;   //reset change tracking and return
                list.Add(obj);
            }
            return list;
        }

        /// <summary>
        /// Specify a custom table name mapper based on the POCO type name
        /// </summary>
        public static TableNameMapperDelegate TableNameMapper;

        private static string GetTableName(Type type)
        {
            string name;
            if (TypeTableName.TryGetValue(type.TypeHandle, out name)) return name;

            if (TableNameMapper != null)
            {
                name = TableNameMapper(type);
            }
            else
            {
                //NOTE: This as dynamic trick should be able to handle both our own Table-attribute as well as the one in EntityFramework 
                var tableAttr = type.GetCustomAttributes(false).SingleOrDefault(attr => attr.GetType().Name == "TableAttribute") as dynamic;
                if (tableAttr != null)
                    name = tableAttr.Name;
                else
                {
                    name = type.Name + "s";
                    if (type.IsInterface && name.StartsWith("I"))
                        name = name.Substring(1);
                }
            }

            TypeTableName[type.TypeHandle] = name;
            return name;
        }


        /// <summary>
        /// Inserts an entity into table "Ts" and returns identity id or number if inserted rows if inserting a list.
        /// </summary>
        /// <param name="connection">Open SqlConnection</param>
        /// <param name="entityToInsert">Entity to insert, can be list of entities</param>
        /// <returns>Identity of inserted entity, or number of inserted rows if inserting a list</returns>
        public static long Insert<T>(this IDbConnection connection, T entityToInsert, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
        {
            var isList = false;

            var type = typeof(T);

            if (type.IsArray || type.IsGenericType)
            {
                isList = true;
                type = type.GetGenericArguments()[0];
            }

            var name = GetTableName(type);
            var sbColumnList = new StringBuilder(null);
            var allProperties = TypePropertiesCache(type);
            var keyProperties = KeyPropertiesCache(type);
            var computedProperties = ComputedPropertiesCache(type);
            var allPropertiesExceptKeyAndComputed = allProperties.Except(keyProperties.Union(computedProperties)).ToList();

            var adapter = GetFormatter(connection);

            for (var i = 0; i < allPropertiesExceptKeyAndComputed.Count(); i++)
            {
                var property = allPropertiesExceptKeyAndComputed.ElementAt(i);
                adapter.AppendColumnName(sbColumnList, property.Name);  //fix for issue #336
                if (i < allPropertiesExceptKeyAndComputed.Count() - 1)
                    sbColumnList.Append(", ");
            }

            var sbParameterList = new StringBuilder(null);
            for (var i = 0; i < allPropertiesExceptKeyAndComputed.Count(); i++)
            {
                var property = allPropertiesExceptKeyAndComputed.ElementAt(i);
                sbParameterList.AppendFormat("@{0}", property.Name);
                if (i < allPropertiesExceptKeyAndComputed.Count() - 1)
                    sbParameterList.Append(", ");
            }

            int returnVal;
            var wasClosed = connection.State == ConnectionState.Closed;
            if (wasClosed) connection.Open();

            if (!isList)    //single entity
            {
                returnVal = adapter.Insert(connection, transaction, commandTimeout, name, sbColumnList.ToString(),
                    sbParameterList.ToString(), keyProperties, entityToInsert);
            }
            else
            {
                //insert list of entities
                var cmd = String.Format("insert into {0} ({1}) values ({2})", name, sbColumnList, sbParameterList);
                returnVal = connection.Execute(cmd, entityToInsert, transaction, commandTimeout);
            }
            if (wasClosed) connection.Close();
            return returnVal;
        }

        /// <summary>
        /// Updates entity in table "Ts", checks if the entity is modified if the entity is tracked by the Get() extension.
        /// </summary>
        /// <typeparam name="T">Type to be updated</typeparam>
        /// <param name="connection">Open SqlConnection</param>
        /// <param name="entityToUpdate">Entity to be updated</param>
        /// <returns>true if updated, false if not found or not modified (tracked entities)</returns>
        public static bool Update<T>(this IDbConnection connection, T entityToUpdate, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
        {
            var proxy = entityToUpdate as IProxy;
            if (proxy != null)
            {
                if (!proxy.IsDirty) return false;
            }

            var type = typeof(T);

            if (type.IsArray || type.IsGenericType)
                type = type.GetGenericArguments()[0];

            var keyProperties = KeyPropertiesCache(type);
            var explicitKeyProperties = ExplicitKeyPropertiesCache(type);
            if (!keyProperties.Any() && !explicitKeyProperties.Any())
                throw new ArgumentException("Entity must have at least one [Key] or [ExplicitKey] property");


            var name = GetTableName(type);

            var sb = new StringBuilder();
            sb.AppendFormat("update {0} set ", name);

            var allProperties = TypePropertiesCache(type);
            keyProperties.AddRange(explicitKeyProperties);
            var computedProperties = ComputedPropertiesCache(type);
            var nonIdProps = allProperties.Except(keyProperties.Union(computedProperties)).ToList();

            var adapter = GetFormatter(connection);

            for (var i = 0; i < nonIdProps.Count(); i++)
            {
                var property = nonIdProps.ElementAt(i);
                adapter.AppendColumnNameEqualsValue(sb, property.Name);  //fix for issue #336
                if (i < nonIdProps.Count() - 1)
                    sb.AppendFormat(", ");
            }
            sb.Append(" where ");
            for (var i = 0; i < keyProperties.Count(); i++)
            {
                var property = keyProperties.ElementAt(i);
                adapter.AppendColumnNameEqualsValue(sb, property.Name);  //fix for issue #336
                if (i < keyProperties.Count() - 1)
                    sb.AppendFormat(" and ");
            }
            var updated = connection.Execute(sb.ToString(), entityToUpdate, commandTimeout: commandTimeout, transaction: transaction);
            return updated > 0;
        }

        /// <summary>
        /// Delete entity in table "Ts".
        /// </summary>
        /// <typeparam name="T">Type of entity</typeparam>
        /// <param name="connection">Open SqlConnection</param>
        /// <param name="entityToDelete">Entity to delete</param>
        /// <returns>true if deleted, false if not found</returns>
        public static bool Delete<T>(this IDbConnection connection, T entityToDelete, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
        {
            if (entityToDelete == null)
                throw new ArgumentException("Cannot Delete null Object", "entityToDelete");

            var type = typeof(T);

            if (type.IsArray || type.IsGenericType)
                type = type.GetGenericArguments()[0];

            var keyProperties = KeyPropertiesCache(type);
            var explicitKeyProperties = ExplicitKeyPropertiesCache(type);
            if (!keyProperties.Any() && !explicitKeyProperties.Any())
                throw new ArgumentException("Entity must have at least one [Key] or [ExplicitKey] property");

            var name = GetTableName(type);
            keyProperties.AddRange(explicitKeyProperties);

            var sb = new StringBuilder();
            sb.AppendFormat("delete from {0} where ", name);

            var adapter = GetFormatter(connection);

            for (var i = 0; i < keyProperties.Count(); i++)
            {
                var property = keyProperties.ElementAt(i);
                adapter.AppendColumnNameEqualsValue(sb, property.Name);  //fix for issue #336
                if (i < keyProperties.Count() - 1)
                    sb.AppendFormat(" and ");
            }
            var deleted = connection.Execute(sb.ToString(), entityToDelete, transaction, commandTimeout);
            return deleted > 0;
        }

        /// <summary>
        /// Delete all entities in the table related to the type T.
        /// </summary>
        /// <typeparam name="T">Type of entity</typeparam>
        /// <param name="connection">Open SqlConnection</param>
        /// <returns>true if deleted, false if none found</returns>
        public static bool DeleteAll<T>(this IDbConnection connection, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
        {
            var type = typeof(T);
            var name = GetTableName(type);
            var statement = String.Format("delete from {0}", name);
            var deleted = connection.Execute(statement, null, transaction, commandTimeout);
            return deleted > 0;
        }

        /// <summary>
        /// Specifies a custom callback that detects the database type instead of relying on the default strategy (the name of the connection type object).
        /// Please note that this callback is global and will be used by all the calls that require a database specific adapter.
        /// </summary>
        public static GetDatabaseTypeDelegate GetDatabaseType;


        private static ISqlAdapter GetFormatter(IDbConnection connection)
        {
            string name;
            if (GetDatabaseType != null)
            {
                name = GetDatabaseType(connection);
                if (name != null)
                    name = name.ToLower();
            }
            else
            {
                name = connection.GetType().Name.ToLower();
            }

            return !AdapterDictionary.ContainsKey(name) ?
                new SqlServerAdapter() :
                AdapterDictionary[name];
        }

        static class ProxyGenerator
        {
            private static readonly Dictionary<Type, object> TypeCache = new Dictionary<Type, object>();

            private static AssemblyBuilder GetAsmBuilder(string name)
            {
                var assemblyBuilder = Thread.GetDomain().DefineDynamicAssembly(new AssemblyName { Name = name },
                    AssemblyBuilderAccess.Run);       //NOTE: to save, use RunAndSave

                return assemblyBuilder;
            }

            public static T GetInterfaceProxy<T>()
            {
                Type typeOfT = typeof(T);

                object k;
                if (TypeCache.TryGetValue(typeOfT, out k))
                {
                    return (T)k;
                }
                var assemblyBuilder = GetAsmBuilder(typeOfT.Name);

                var moduleBuilder = assemblyBuilder.DefineDynamicModule("SqlMapperExtensions." + typeOfT.Name); //NOTE: to save, add "asdasd.dll" parameter

                var interfaceType = typeof(IProxy);
                var typeBuilder = moduleBuilder.DefineType(typeOfT.Name + "_" + Guid.NewGuid(),
                    TypeAttributes.Public | TypeAttributes.Class);
                typeBuilder.AddInterfaceImplementation(typeOfT);
                typeBuilder.AddInterfaceImplementation(interfaceType);

                //create our _isDirty field, which implements IProxy
                var setIsDirtyMethod = CreateIsDirtyProperty(typeBuilder);

                // Generate a field for each property, which implements the T
                foreach (var property in typeof(T).GetProperties())
                {
                    var isId = property.GetCustomAttributes(true).Any(a => a is KeyAttribute);
                    CreateProperty<T>(typeBuilder, property.Name, property.PropertyType, setIsDirtyMethod, isId);
                }

                var generatedType = typeBuilder.CreateType();

                //assemblyBuilder.Save(name + ".dll");  //NOTE: to save, uncomment

                var generatedObject = Activator.CreateInstance(generatedType);

                TypeCache.Add(typeOfT, generatedObject);
                return (T)generatedObject;
            }


            private static MethodInfo CreateIsDirtyProperty(TypeBuilder typeBuilder)
            {
                var propType = typeof(bool);
                var field = typeBuilder.DefineField("_" + "IsDirty", propType, FieldAttributes.Private);
                var property = typeBuilder.DefineProperty("IsDirty",
                                               System.Reflection.PropertyAttributes.None,
                                               propType,
                                               new[] { propType });

                const MethodAttributes getSetAttr = MethodAttributes.Public | MethodAttributes.NewSlot | MethodAttributes.SpecialName |
                                                    MethodAttributes.Final | MethodAttributes.Virtual | MethodAttributes.HideBySig;

                // Define the "get" and "set" accessor methods
                var currGetPropMthdBldr = typeBuilder.DefineMethod("get_" + "IsDirty",
                                             getSetAttr,
                                             propType,
                                             Type.EmptyTypes);
                var currGetIl = currGetPropMthdBldr.GetILGenerator();
                currGetIl.Emit(OpCodes.Ldarg_0);
                currGetIl.Emit(OpCodes.Ldfld, field);
                currGetIl.Emit(OpCodes.Ret);
                var currSetPropMthdBldr = typeBuilder.DefineMethod("set_" + "IsDirty",
                                             getSetAttr,
                                             null,
                                             new[] { propType });
                var currSetIl = currSetPropMthdBldr.GetILGenerator();
                currSetIl.Emit(OpCodes.Ldarg_0);
                currSetIl.Emit(OpCodes.Ldarg_1);
                currSetIl.Emit(OpCodes.Stfld, field);
                currSetIl.Emit(OpCodes.Ret);

                property.SetGetMethod(currGetPropMthdBldr);
                property.SetSetMethod(currSetPropMthdBldr);
                var getMethod = typeof(IProxy).GetMethod("get_" + "IsDirty");
                var setMethod = typeof(IProxy).GetMethod("set_" + "IsDirty");
                typeBuilder.DefineMethodOverride(currGetPropMthdBldr, getMethod);
                typeBuilder.DefineMethodOverride(currSetPropMthdBldr, setMethod);

                return currSetPropMthdBldr;
            }

            private static void CreateProperty<T>(TypeBuilder typeBuilder, string propertyName, Type propType, MethodInfo setIsDirtyMethod, bool isIdentity)
            {
                //Define the field and the property 
                var field = typeBuilder.DefineField("_" + propertyName, propType, FieldAttributes.Private);
                var property = typeBuilder.DefineProperty(propertyName,
                                               System.Reflection.PropertyAttributes.None,
                                               propType,
                                               new[] { propType });

                const MethodAttributes getSetAttr = MethodAttributes.Public | MethodAttributes.Virtual |
                                                    MethodAttributes.HideBySig;

                // Define the "get" and "set" accessor methods
                var currGetPropMthdBldr = typeBuilder.DefineMethod("get_" + propertyName,
                                             getSetAttr,
                                             propType,
                                             Type.EmptyTypes);

                var currGetIl = currGetPropMthdBldr.GetILGenerator();
                currGetIl.Emit(OpCodes.Ldarg_0);
                currGetIl.Emit(OpCodes.Ldfld, field);
                currGetIl.Emit(OpCodes.Ret);

                var currSetPropMthdBldr = typeBuilder.DefineMethod("set_" + propertyName,
                                             getSetAttr,
                                             null,
                                             new[] { propType });

                //store value in private field and set the isdirty flag
                var currSetIl = currSetPropMthdBldr.GetILGenerator();
                currSetIl.Emit(OpCodes.Ldarg_0);
                currSetIl.Emit(OpCodes.Ldarg_1);
                currSetIl.Emit(OpCodes.Stfld, field);
                currSetIl.Emit(OpCodes.Ldarg_0);
                currSetIl.Emit(OpCodes.Ldc_I4_1);
                currSetIl.Emit(OpCodes.Call, setIsDirtyMethod);
                currSetIl.Emit(OpCodes.Ret);

                //TODO: Should copy all attributes defined by the interface?
                if (isIdentity)
                {
                    var keyAttribute = typeof(KeyAttribute);
                    var myConstructorInfo = keyAttribute.GetConstructor(new Type[] { });
                    var attributeBuilder = new CustomAttributeBuilder(myConstructorInfo, new object[] { });
                    property.SetCustomAttribute(attributeBuilder);
                }

                property.SetGetMethod(currGetPropMthdBldr);
                property.SetSetMethod(currSetPropMthdBldr);
                var getMethod = typeof(T).GetMethod("get_" + propertyName);
                var setMethod = typeof(T).GetMethod("set_" + propertyName);
                typeBuilder.DefineMethodOverride(currGetPropMthdBldr, getMethod);
                typeBuilder.DefineMethodOverride(currSetPropMthdBldr, setMethod);
            }
        }

    }

    [AttributeUsage(AttributeTargets.Class)]
    public class TableAttribute : Attribute
    {
        public TableAttribute(string tableName)
        {
            Name = tableName;
        }

        // ReSharper disable once MemberCanBePrivate.Global
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public string Name { get; set; }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class ExplicitKeyAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class WriteAttribute : Attribute
    {
        public WriteAttribute(bool write)
        {
            Write = write;
        }
        public bool Write { get; private set; }
    }
}

public partial interface ISqlAdapter
{
    int Insert(IDbConnection connection, IDbTransaction transaction, int? commandTimeout, String tableName, string columnList, string parameterList, IEnumerable<PropertyInfo> keyProperties, object entityToInsert);

    //new methods for issue #336
    void AppendColumnName(StringBuilder sb, string columnName);
    void AppendColumnNameEqualsValue(StringBuilder sb, string columnName);
}

public partial class SqlServerAdapter : ISqlAdapter
{
    public int Insert(IDbConnection connection, IDbTransaction transaction, int? commandTimeout, String tableName, string columnList, string parameterList, IEnumerable<PropertyInfo> keyProperties, object entityToInsert)
    {
        var cmd = String.Format("insert into {0} ({1}) values ({2});select SCOPE_IDENTITY() id", tableName, columnList, parameterList);
        var multi = connection.QueryMultiple(cmd, entityToInsert, transaction, commandTimeout);

        var first = multi.Read().FirstOrDefault();
        if (first == null || first.id == null) return 0;

        var id = (int)first.id;
        var propertyInfos = keyProperties as PropertyInfo[] ?? keyProperties.ToArray();
        if (!propertyInfos.Any()) return id;

        var idProperty = propertyInfos.First();
        idProperty.SetValue(entityToInsert, Convert.ChangeType(id, idProperty.PropertyType), null);

        return id;
    }

    public void AppendColumnName(StringBuilder sb, string columnName)
    {
        sb.AppendFormat("[{0}]", columnName);
    }

    public void AppendColumnNameEqualsValue(StringBuilder sb, string columnName)
    {
        sb.AppendFormat("[{0}] = @{1}", columnName, columnName);
    }
}

public partial class SqlCeServerAdapter : ISqlAdapter
{
    public int Insert(IDbConnection connection, IDbTransaction transaction, int? commandTimeout, String tableName, string columnList, string parameterList, IEnumerable<PropertyInfo> keyProperties, object entityToInsert)
    {
        var cmd = String.Format("insert into {0} ({1}) values ({2})", tableName, columnList, parameterList);
        connection.Execute(cmd, entityToInsert, transaction, commandTimeout);
        var r = connection.Query("select @@IDENTITY id", transaction: transaction, commandTimeout: commandTimeout).ToList();

        if (r.First().id == null) return 0;
        var id = (int)r.First().id;

        var propertyInfos = keyProperties as PropertyInfo[] ?? keyProperties.ToArray();
        if (!propertyInfos.Any()) return id;

        var idProperty = propertyInfos.First();
        idProperty.SetValue(entityToInsert, Convert.ChangeType(id, idProperty.PropertyType), null);

        return id;
    }

    public void AppendColumnName(StringBuilder sb, string columnName)
    {
        sb.AppendFormat("[{0}]", columnName);
    }

    public void AppendColumnNameEqualsValue(StringBuilder sb, string columnName)
    {
        sb.AppendFormat("[{0}] = @{1}", columnName, columnName);
    }
}

public partial class MySqlAdapter : ISqlAdapter
{
    public int Insert(IDbConnection connection, IDbTransaction transaction, int? commandTimeout, String tableName, string columnList, string parameterList, IEnumerable<PropertyInfo> keyProperties, object entityToInsert)
    {
        var cmd = String.Format("insert into {0} ({1}) values ({2})", tableName, columnList, parameterList);
        connection.Execute(cmd, entityToInsert, transaction, commandTimeout);
        var r = connection.Query("Select LAST_INSERT_ID()", transaction: transaction, commandTimeout: commandTimeout);

        var id = r.First().id;
        if (id == null) return 0;
        var propertyInfos = keyProperties as PropertyInfo[] ?? keyProperties.ToArray();
        if (!propertyInfos.Any()) return id;

        var idp = propertyInfos.First();
        idp.SetValue(entityToInsert, Convert.ChangeType(id, idp.PropertyType), null);

        return id;
    }

    public void AppendColumnName(StringBuilder sb, string columnName)
    {
        sb.AppendFormat("`{0}`", columnName);
    }

    public void AppendColumnNameEqualsValue(StringBuilder sb, string columnName)
    {
        sb.AppendFormat("`{0}` = @{1}", columnName, columnName);
    }
}


public partial class PostgresAdapter : ISqlAdapter
{
    public int Insert(IDbConnection connection, IDbTransaction transaction, int? commandTimeout, String tableName, string columnList, string parameterList, IEnumerable<PropertyInfo> keyProperties, object entityToInsert)
    {
        var sb = new StringBuilder();
        sb.AppendFormat("insert into {0} ({1}) values ({2})", tableName, columnList, parameterList);

        // If no primary key then safe to assume a join table with not too much data to return
        var propertyInfos = keyProperties as PropertyInfo[] ?? keyProperties.ToArray();
        if (!propertyInfos.Any())
            sb.Append(" RETURNING *");
        else
        {
            sb.Append(" RETURNING ");
            var first = true;
            foreach (var property in propertyInfos)
            {
                if (!first)
                    sb.Append(", ");
                first = false;
                sb.Append(property.Name);
            }
        }

        var results = connection.Query(sb.ToString(), entityToInsert, transaction, commandTimeout: commandTimeout).ToList();

        // Return the key by assinging the corresponding property in the object - by product is that it supports compound primary keys
        var id = 0;
        foreach (var p in propertyInfos)
        {
            var value = ((IDictionary<string, object>)results.First())[p.Name.ToLower()];
            p.SetValue(entityToInsert, value, null);
            if (id == 0)
                id = Convert.ToInt32(value);
        }
        return id;
    }

    public void AppendColumnName(StringBuilder sb, string columnName)
    {
        sb.AppendFormat("\"{0}\"", columnName);
    }

    public void AppendColumnNameEqualsValue(StringBuilder sb, string columnName)
    {
        sb.AppendFormat("\"{0}\" = @{1}", columnName, columnName);
    }
}

public partial class SQLiteAdapter : ISqlAdapter
{
    public int Insert(IDbConnection connection, IDbTransaction transaction, int? commandTimeout, String tableName, string columnList, string parameterList, IEnumerable<PropertyInfo> keyProperties, object entityToInsert)
    {
        var cmd = String.Format("insert into {0} ({1}) values ({2}); select last_insert_rowid() id", tableName, columnList, parameterList);
        var multi = connection.QueryMultiple(cmd, entityToInsert, transaction, commandTimeout);

        var id = (int)multi.Read().First().id;
        var propertyInfos = keyProperties as PropertyInfo[] ?? keyProperties.ToArray();
        if (!propertyInfos.Any()) return id;

        var idProperty = propertyInfos.First();
        idProperty.SetValue(entityToInsert, Convert.ChangeType(id, idProperty.PropertyType), null);

        return id;
    }

    public void AppendColumnName(StringBuilder sb, string columnName)
    {
        sb.AppendFormat("\"{0}\"", columnName);
    }

    public void AppendColumnNameEqualsValue(StringBuilder sb, string columnName)
    {
        sb.AppendFormat("\"{0}\" = @{1}", columnName, columnName);
    }
}
﻿
namespace Model.Empregado.Status
{
    public sealed class StatusAtivo : EmpregadoStatus
    {
        public override EmpregadoStatusTipo Status
        {
            get { return EmpregadoStatusTipo.Ativo; }
        }

        public override void Notifica(Empregado empregado)
        {
            //DomainEvents.Raise(new AtivaEmpregadoEvent(empregado));
            empregado.SetStatus(new StatusAtivo());
        }
    }
}
﻿
using Model.Empregado.Events;
using Infrastructure.Domain.Events;

namespace Model.Empregado.Status
{
    public sealed class StatusInativo : EmpregadoStatus
    {
        public override EmpregadoStatusTipo Status
        {
            get { return EmpregadoStatusTipo.Inativo; }
        }

        public override void Notifica(Empregado empregado)
        {
            DomainEvents.Raise(new InativaEmpregadoEvent(empregado));
            empregado.SetStatus(new StatusInativo());
        }
    }
}
﻿
namespace Model.Empregado.Status
{
    public sealed class StatusIndefinido: EmpregadoStatus
    {
        public override EmpregadoStatusTipo Status
        {
            get { return EmpregadoStatusTipo.Indefinido; }
        }

        public override void Notifica(Empregado empregado)
        {
            empregado.SetStatus(new StatusIndefinido());
        }
    }
}
﻿

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc.Util
{
    public static class Util
    {
        public static Dictionary<string, string> Teste()
        {
            Dictionary<string, string> i = new Dictionary<string, string>();
            items.Add("S", "S");
            items.Add("N", "N");
            return i;
        }
        public static Dictionary<string, string> Tipo()
        {
            Dictionary<string, string> items = new Dictionary<string, string>();
            items.Add("X", "X");

            return items;
        }
        public static Dictionary<string, string> Tipo()
        {
            Dictionary<string, string> i = new Dictionary<string, string>();
            items.Add("X", "X");
            return i;
        }
    }
}﻿
namespace Infrastructure.Domain
{
    public class ValidationError
    {
        public string PropertyName { get; set; }
        public string Message { get; set; }
        public ValidationError(string propertyName, string message)
        {
            this.PropertyName = propertyName;
            this.Message = message;
        }
    }
}
﻿using System.Collections.Generic;

namespace Infrastructure.Domain
{
    public class ValidationErrors
    {
        private List<ValidationError> _errors;

        public ValidationErrors()
        {
            _errors = new List<ValidationError>();
        }

        public IList<ValidationError> Items 
        {
            get { return _errors; } 
        }

        public void Add(string propertyName)
        {
            _errors.Add(new ValidationError(propertyName, propertyName + " is required"));
        }

        public void Add(string propertyName, string errorMessage)
        {
            _errors.Add(new ValidationError(propertyName, errorMessage));
        }

        public void Add(ValidationError error)
        {
            _errors.Add(error);
        }
        public void AddRange(IList<ValidationError> errors)
        {
            _errors.AddRange(errors);
        }

        internal void Clear()
        {
            _errors.Clear();
        }
    }
}
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Newtonsoft.Json.Serialization;

namespace Mvc
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
