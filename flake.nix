{
  description = "A Nix-flake-based C# development environment";

  inputs = {
    nixpkgs.url = "github:nixos/nixpkgs/nixpkgs-unstable";
  };

  outputs = { self, nixpkgs }:
    let
      supportedSystems = [ "x86_64-linux" "aarch64-linux" "x86_64-darwin" "aarch64-darwin" ];
      forEachSupportedSystem = f: nixpkgs.lib.genAttrs supportedSystems (system: f {
        pkgs = import nixpkgs { inherit system; };
      });
    in
    {
      devShells = forEachSupportedSystem ({ pkgs }: {
        default = pkgs.mkShell {
          packages = [
            pkgs.dotnetCorePackages.dotnet_9.sdk
            pkgs.omnisharp-roslyn
            # pkgs.mono
            # pkgs.msbuild
          ];
        };

        environment.sessionVariables = {
          DOTNET_ROOT = "${pkgs.dotnetCorePackages.dotnet_9.sdk}";
        };
      });
    };
}
