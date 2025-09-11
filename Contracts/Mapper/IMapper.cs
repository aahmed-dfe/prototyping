namespace Contracts.Mapper;

public interface IMapper<in TIn, out TOut>
{
    TOut Map(TIn input);
}
