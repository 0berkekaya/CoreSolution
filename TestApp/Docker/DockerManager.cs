using Docker.DotNet.Models;
using Docker.DotNet;
using System.Runtime.InteropServices;
using TestApp.Docker.Models;
using TestApp.Docker.Enums;
using AngleSharp.Text;

namespace TestApp.Docker
{
    public class DockerManager : IDockerManager
    {
        private readonly DockerClient _client;

        public DockerManager()
        {
            _client = new DockerClientConfiguration(GetDockerUri()).CreateClient();
        }
        public DockerManager(DockerClientConfiguration configuration)
        {
            _client = configuration.CreateClient();
        }

        public async Task<string> CreateContainerAsync(ContainerName containerName, Dictionary<string, string>? envVariables = null, string? command = null, bool latest = true)
        {
            string imageName = containerName.ToString().ToLower();
            IEnumerable<ContainerInfo> allContainers = await GetContainersAsync(containerName);
            List<int> usedPorts = (allContainers.Select(c => c.Name.Split('_')[1]).ToList()).Select(c => c.ToInteger(1)).ToList();

            int containerPort = GetDefaultPortForContainer(containerName);

            int hostPort = usedPorts.Count > 0 ? usedPorts.Max() + 1 : 1;

            string containerNameWithCount = $"{containerName}_{hostPort}";

            CreateContainerParameters containerConfig = new CreateContainerParameters
            {
                Image = imageName + (latest ? ":latest" : ""),
                Cmd = command != null ? new[] { command } : null,
                ExposedPorts = new Dictionary<string, EmptyStruct>
            {
                { $"{containerPort}/tcp", default }
            },
                HostConfig = new HostConfig
                {
                    PortBindings = new Dictionary<string, IList<PortBinding>>
                {
                    {
                        $"{containerPort}/tcp",
                        new List<PortBinding>
                        {
                            new PortBinding
                            {
                                HostIP = "0.0.0.0",
                                HostPort = hostPort.ToString()
                            }
                        }
                    }
                }
                },
                Name = containerNameWithCount,
                Env = envVariables?.Select(kv => $"{kv.Key}={kv.Value}").ToList()
            };

            CreateContainerResponse createResponse = await _client.Containers.CreateContainerAsync(containerConfig);
            return createResponse.ID;
        }

        public async Task StartContainerWithIdAsync(string containerId)
        {
            await _client.Containers.StartContainerAsync(containerId, new ContainerStartParameters());
        }

        public async Task StartContainerWithPrefixAsync(ContainerName containerName)
        {
            IList<ContainerListResponse> containers = await _client.Containers.ListContainersAsync(new ContainersListParameters() { All = true });
            IEnumerable<ContainerListResponse> filteredContainers = containers.Where(c => c.Names.Any(name => name.StartsWith($"/{containerName.ToString()}")));

            foreach (ContainerListResponse container in filteredContainers)
                await _client.Containers.StartContainerAsync(container.ID, new ContainerStartParameters());
        }

        public async Task StartAllContainersAsync()
        {
            IList<ContainerListResponse> containers = await _client.Containers.ListContainersAsync(new ContainersListParameters() { All = true });

            foreach (ContainerListResponse container in containers)
                await _client.Containers.StartContainerAsync(container.ID, new ContainerStartParameters());
        }

        public async Task StopContainerWithIdAsync(string containerId)
        {
            await _client.Containers.StopContainerAsync(containerId, new ContainerStopParameters());
        }

        public async Task StopContainerWithPrefixAsync(ContainerName containerName)
        {
            IList<ContainerListResponse> containers = await _client.Containers.ListContainersAsync(new ContainersListParameters() { All = true });
            IEnumerable<ContainerListResponse> filteredContainers = containers.Where(c => c.Names.Any(name => name.StartsWith($"/{containerName.ToString()}")));

            foreach (ContainerListResponse container in filteredContainers)
                await _client.Containers.StopContainerAsync(container.ID, new ContainerStopParameters());
        }

        public async Task StopAllContainersAsync()
        {
            IList<ContainerListResponse> containers = await _client.Containers.ListContainersAsync(new ContainersListParameters() { All = true });

            foreach (ContainerListResponse container in containers)
                await _client.Containers.StopContainerAsync(container.ID, new ContainerStopParameters());
        }

        public async Task RestartContainerAsync(bool allContainers, string? containerId = null)
        {
            if (allContainers)
            {
                IList<ContainerListResponse> containers = await _client.Containers.ListContainersAsync(new ContainersListParameters() { All = true });
                foreach (ContainerListResponse container in containers)
                    await _client.Containers.RestartContainerAsync(container.ID, new ContainerRestartParameters());
            }
            else
            {
                if (containerId != null)
                    await _client.Containers.RestartContainerAsync(containerId, new ContainerRestartParameters());
            }
        }

        public async Task DeleteContainerAsync(string containerId)
        {
            await StopContainerWithIdAsync(containerId);
            await _client.Containers.RemoveContainerAsync(containerId, new ContainerRemoveParameters());
        }

        public async Task DeleteAllContainersWithContainerNameAsync(ContainerName containerNamePrefix)
        {
            var containers = await GetContainersAsync(containerNamePrefix);
            foreach (var container in containers)
            {
                await StopContainerWithIdAsync(container.ContainerId);
                await _client.Containers.RemoveContainerAsync(container.ContainerId, new ContainerRemoveParameters());

            }
        }

        public async Task<IEnumerable<ContainerInfo>> GetContainersAsync(ContainerName containerName, ContainersListParameters? containersListParameters = null)
        {
            IList<ContainerListResponse> containers = await _client.Containers.ListContainersAsync(containersListParameters ?? new ContainersListParameters() { All = true });
            return containers
                .Where(c => c.Names.Any(name => name.StartsWith($"/{containerName}")))
                .Select(c => new ContainerInfo
                {
                    ContainerId = c.ID,
                    Name = c.Names.FirstOrDefault(name => name.StartsWith($"/{containerName}"))?.Substring(1) ?? string.Empty,
                    PrivatePort = c.Ports.FirstOrDefault()?.PrivatePort ?? 0,
                    PublicPort = c.Ports.FirstOrDefault()?.PublicPort ?? 0,
                    State = c.State,
                });
        }

        public async Task<IEnumerable<ContainerInfo>> GetAllContainersAsync(ContainersListParameters? containersListParameters = null)
        {
            IList<ContainerListResponse> containers = await _client.Containers.ListContainersAsync(containersListParameters ?? new ContainersListParameters() { All = true });
            return containers
                .Select(c => new ContainerInfo
                {
                    ContainerId = c.ID,
                    Name = c.Names.FirstOrDefault() ?? string.Empty,
                    PrivatePort = c.Ports.FirstOrDefault()?.PrivatePort ?? 0,
                    PublicPort = c.Ports.FirstOrDefault()?.PublicPort ?? 0,
                    State = c.State,
                });
        }

        private static Uri GetDockerUri()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) return new Uri("npipe://./pipe/docker_engine");
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) return new Uri("unix:///var/run/docker.sock");
            else throw new NotSupportedException("Desteklenmeyen işletim sistemi.");
        }

        private int GetDefaultPortForContainer(ContainerName containerName)
        {
            return containerName switch
            {
                ContainerName.Redis => 6379,
                ContainerName.PostgreSql => 5432,
                _ => 80 // Diğer imajlar için varsayılan port (genellikle web uygulamaları için 80)
            };
        }
    }
}
