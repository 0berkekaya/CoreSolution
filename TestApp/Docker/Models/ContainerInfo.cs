
namespace TestApp.Docker.Models
{
    public class ContainerInfo
    {
        public string ContainerId { get; set; }
        public string Name { get; set; }
        public ushort PrivatePort { get; set; }
        public ushort PublicPort { get; set; }
        public string State { get; set; }
    }
}
