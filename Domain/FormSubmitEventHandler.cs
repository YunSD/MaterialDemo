using MaterialDemo.Config.Db;

namespace MaterialDemo.Domain;


public delegate void FormSubmitEventHandler<T>(T entity) where T : BaseEntity;
