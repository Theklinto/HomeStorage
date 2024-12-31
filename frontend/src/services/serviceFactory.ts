export interface IService {}

const _services = new Map<ConstructorlessService<IService>, IService>();
type ConstructorlessService<TService extends IService> = new () => TService;

export function getService<TService extends IService>(
    service: ConstructorlessService<TService>
): TService {
    let foundService: TService = _services.get(service) as TService;
    if (!foundService) {
        _services.set(service, service);
        foundService = service as unknown as TService;
    }

    return foundService;
}
