namespace Contracts.Mapper.Mapper;

public interface IMapper<in TIn, out TOut>
{
    TOut Map(TIn input);
}
