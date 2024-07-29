using Docker.DotNet.Models;
using TestApp.Docker.Enums;
using TestApp.Docker.Models;

namespace TestApp.Docker
{
    public interface IDockerManager
    {
        Task<string> CreateContainerAsync(ContainerName containerName, Dictionary<string, string>? envVariables = null, string? command = null, bool latest = true);
        Task StartContainerWithIdAsync(string containerId);
        Task StartContainerWithPrefixAsync(ContainerName containerName);
        Task StartAllContainersAsync();
        Task StopContainerWithIdAsync(string containerId);
        Task StopContainerWithPrefixAsync(ContainerName containerName);
        Task StopAllContainersAsync();
        Task RestartContainerAsync(bool allContainers, string? containerId = null);
        Task DeleteContainerAsync(string containerId);
        Task DeleteAllContainersWithContainerNameAsync(ContainerName containerNamePrefix);
        Task<IEnumerable<ContainerInfo>> GetContainersAsync(ContainerName containerName, ContainersListParameters? containersListParameters = null);
    }
}
