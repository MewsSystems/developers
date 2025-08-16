using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;

var config = DefaultConfig.Instance;
var summaries = BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args, config);