using AutoMapper;
using MetricsAgent.Models;
using MetricsAgent.Models.Dto;
using MetricsAgent.Models.Requests;

namespace MetricsAgent.Mappings
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            //CpuMetric
            CreateMap<CpuMetricCreateRequest, CpuMetric>()
                .ForMember(x => x.Time,
                opt => opt.MapFrom(src => (long)src.Time.TotalSeconds));

            CreateMap<CpuMetric, CpuMetricDto>();


            //DotnetMetric
            CreateMap<DotNetMetricCreateRequest, DotNetMetric>()
                .ForMember(x => x.Time,
                opt => opt.MapFrom(src => (long)src.Time.TotalSeconds));

            CreateMap<DotNetMetric, DotNetMetricDto>();



            //HddMetric
            CreateMap<HddMetricCreateRequest, HddMetric>()
                .ForMember(x => x.Time,
                opt => opt.MapFrom(src => (long)src.Time.TotalSeconds));

            CreateMap<HddMetric, HddMetricDto>();



            //NetworkMetric
            CreateMap<NetworkMetricCreateRequest, NetworkMetric>()
                .ForMember(x => x.Time,
                opt => opt.MapFrom(src => (long)src.Time.TotalSeconds));

            CreateMap<NetworkMetric, NetworkMetricDto>();



            //RamMetric
            CreateMap<RamMetricCreateRequest, RamMertic>()
                .ForMember(x => x.Time,
                opt => opt.MapFrom(src => (long)src.Time.TotalSeconds));

            CreateMap<RamMertic, RamMetricDto>();


        }

    }
}
