#!/bin/bash

echo "🚀 Setting up .NET 9 + SvelteKit development environment..."

# Verify .NET 9 installation
echo "📦 Verifying .NET 9 installation..."
dotnet --version

# Install global .NET tools
echo "🔧 Installing .NET global tools..."
dotnet tool install -g dotnet-ef
dotnet tool install -g dotnet-aspnet-codegenerator
dotnet tool install -g Microsoft.Web.LibraryManager.Cli

# Update npm to latest
echo "📦 Updating npm..."
npm install -g npm@latest

# Install common global packages for SvelteKit development
echo "🔧 Installing global npm packages..."
npm install -g @sveltejs/kit
npm install -g svelte-check
npm install -g typescript
npm install -g prettier
npm install -g eslint

# Set Git config if not set
if [ -z "$(git config --get user.name)" ]; then
    echo "⚙️  Please configure Git:"
    echo "git config --global user.name 'Luke Jameson'"
    echo "git config --global user.email 'lukejameson@live.co.uk'"
fi

echo "🎉 Setup complete! Happy coding!"
