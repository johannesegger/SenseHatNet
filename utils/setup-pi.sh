# Install docker - https://docs.docker.com/engine/install/debian/#install-using-the-convenience-script
curl -fsSL https://get.docker.com -o get-docker.sh
sudo sh get-docker.sh

# Install dotnet SDK - https://docs.microsoft.com/en-us/dotnet/core/install/linux-debian#manual-install
# ARM64
wget https://download.visualstudio.microsoft.com/download/pr/7a027d45-b442-4cc5-91e5-e5ea210ffc75/68c891aaae18468a25803ff7c105cf18/dotnet-sdk-3.1.403-linux-arm64.tar.gz
# ARM32
wget https://download.visualstudio.microsoft.com/download/pr/8a2da583-cac8-4490-bcca-2a3667d51142/6a0f7fb4b678904cdb79f3cd4d4767d5/dotnet-sdk-3.1.403-linux-arm.tar.gz
mkdir -p "$HOME/dotnet" && tar zxf dotnet-sdk-*.tar.gz -C "$HOME/dotnet"
echo "
export DOTNET_ROOT=\$HOME/dotnet
export PATH=\$PATH:\$HOME/dotnet" >> $HOME/.bashrc
. $HOME/.bashrc

# Install git
sudo apt update && sudo apt install -y git