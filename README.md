# Driving
Selected the Self Driving assignment. I did not add many test because I put a cap of 2 hours of time I worked on this. I thought test could be an interesting concept to discuss further.

# .NET Command Line Interface

[![Join the chat at https://gitter.im/dotnet/cli](https://badges.gitter.im/Join%20Chat.svg)](https://gitter.im/dotnet/cli?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)

This repo contains the source code for cross-platform [.NET Core](http://github.com/dotnet/core) command line toolchain. It contains the implementation of each command, the native packages for various supported platforms as well as documentation. 

New to .NET CLI?
------------
Check out our http://dotnet.github.io/getting-started/ page. 


You can download .NET Core as either an installer (MSI, PKG) or a zip (zip, gzip). You can download the product in two flavours:

- .NET Core - .NET Core runtime and framework
- .NET Core SDK - .NET Core + CLI tools


Basic usage
-----------

When you have the .NET Command Line Interface installed on your OS of choice, you can try it out using some of the samples on the [dotnet/core repo](https://github.com/dotnet/core/tree/master/samples). You can download the sample in a directory, and then you can kick the tires of the CLI.


First, you will need to restore the packages:
	
	dotnet restore
	
This will restore all of the packages that are specified in the project.json file of the given sample.

Then you can either run from source or compile the sample. Running from source is straightforward:
	
	dotnet run
	
Compiling to IL is done using:
	
	dotnet build

This will drop a binary in `./bin/[configuration]/[framework]/[rid]/[binary name]` that you can just run.

For more details, please refer to the [documentation](https://github.com/dotnet/corert/tree/master/Documentation).

  dotnet test- go to testing folder directory and then run this command
