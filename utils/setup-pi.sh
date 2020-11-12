# Install docker - https://docs.docker.com/engine/install/debian/#install-using-the-convenience-script
curl -fsSL https://get.docker.com -o get-docker.sh
sudo sh get-docker.sh

# Install dotnet SDK - https://docs.microsoft.com/en-us/dotnet/core/install/linux-debian#manual-install
# ARM64
wget https://download.visualstudio.microsoft.com/download/pr/27840e8b-d61c-472d-8e11-c16784d40091/ae9780ccda4499405cf6f0924f6f036a/dotnet-sdk-5.0.100-linux-arm64.tar.gz
# ARM32
wget https://download.visualstudio.microsoft.com/download/pr/e8912d3b-483b-4d6f-bd3a-3066b3194313/20f2261fe4e16e55df4bbe03c65a7648/dotnet-sdk-5.0.100-linux-arm.tar.gz
mkdir -p "$HOME/dotnet" && tar zxf dotnet-sdk-*.tar.gz -C "$HOME/dotnet"
echo "
export DOTNET_ROOT=\$HOME/dotnet
export PATH=\$PATH:\$HOME/dotnet" >> $HOME/.bashrc
. $HOME/.bashrc

# Install git
sudo apt update && sudo apt install -y git