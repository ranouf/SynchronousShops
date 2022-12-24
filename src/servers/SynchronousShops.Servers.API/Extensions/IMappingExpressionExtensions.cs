using AutoMapper;
using SynchronousShops.Domains.Core.Identity.Entities;
using SynchronousShops.Libraries.Entities;
using SynchronousShops.Servers.API.Controllers.Dtos.Entities;

namespace SynchronousShops.Servers.API.Extensions
{
    public static class IMappingExpressionExtensions
    {
        public static IMappingExpression<TSource, TDestination> AddCreatedBy<TSource, TDestination>(this IMappingExpression<TSource, TDestination> mapping)
            where TSource : ICreationAudited<User>
            where TDestination : ICreationAuditedDto
        {
            return mapping.ForMember(
                dest => dest.CreatedBy,
                opts => opts.MapFrom(src => src.CreatedByUser.FullName)
            );
        }

        public static IMappingExpression<TSource, TDestination> AddModifiedBy<TSource, TDestination>(this IMappingExpression<TSource, TDestination> mapping)
            where TSource : IModificationAudited<User>
            where TDestination : IModificationAuditedDto
        {
            return mapping.ForMember(
                dest => dest.ModifiedBy,
                opts => opts.MapFrom(src => src.ModifiedByUser.FullName)
            );
        }

        public static IMappingExpression<TSource, TDestination> AddAuditedBy<TSource, TDestination>(this IMappingExpression<TSource, TDestination> mapping)
            where TSource : IAudited<User>
            where TDestination : IAuditedDto
        {
            return mapping
                .AddCreatedBy()
                .AddModifiedBy();
        }

        public static IMappingExpression<TSource, TDestination> AddDeletedBy<TSource, TDestination>(this IMappingExpression<TSource, TDestination> mapping)
            where TSource : IDeletionAudited<User>
            where TDestination : IDeletionAuditedDto
        {
            return mapping.ForMember(
                dest => dest.DeletedBy,
                opts => opts.MapFrom(src => src.DeletedByUser.FullName)
            );
        }

        public static IMappingExpression<TSource, TDestination> AddFullAuditedBy<TSource, TDestination>(this IMappingExpression<TSource, TDestination> mapping)
            where TSource : IAudited<User>, IDeletionAudited<User>
            where TDestination : IAuditedDto, IDeletionAuditedDto
        {
            return mapping
                .AddAuditedBy()
                .AddDeletedBy();
        }
        public static IMappingExpression<TSource, TDestination> SetDiscriminator<TSource, TDestination>(this IMappingExpression<TSource, TDestination> mapping)
            where TSource : class
            where TDestination : IDiscriminatorDto
        {
            return mapping.ForMember(
                dest => dest.Discriminator,
                opts => opts.MapFrom(src => typeof(TDestination).Name)
            );
        }
    }
}
