using WebDevelopment.Shared.RequestFeatures;
using WebDevelopment.Shared.Responses;

namespace WebDevelopment.Shared.Interfaces;

public interface IDataRequestHandlerService<SourceT, DestT>
{
    Response<IEnumerable<DestT>> HandleRequest(IQueryable<SourceT> source, DataRequest request);
}
