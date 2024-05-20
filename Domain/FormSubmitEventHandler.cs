using MaterialDemo.Config.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialDemo.Domain;


public delegate void FormSubmitEventHandler<T>(T entity) where T : BaseEntity;
