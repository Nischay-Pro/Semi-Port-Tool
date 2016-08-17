# Semi Port Tool [![Build Status](https://travis-ci.org/Nischay-Pro/Semi-Port-Tool.svg?branch=test)](https://travis-ci.org/Nischay-Pro/Semi-Port-Tool)
# What is it?
**Semi Port Tool** is a simple software written using .net technologies with cross platform support aimed with one goal; To simplify the Porting for Android ROM's from one device to another.

# How to compile
## Requirements
1. Visual Studio 2015
2. A Windows 7+ OS
3. .net 4.5 framework
4. Droid Serif Font

## Building
1. Open the project using Visual Studio Community. Select the Solution File in the Open Project Dialog
2. Compile the project using standard (default) build parameters and run.

# Reporting Errors and Suggestions
## To Report Errors
Write a brief overview of the error and try writing the steps to reproduce the error. Do include screenshots as well.
Use the Github Issues tab and attach label **"Bug"**

## To Report Suggestions
### General Suggestions
Write a brief overview of the feature you want to be added.
Use the Github Issues tab and attach label **"Enchancement"**

### Device Support
#### Only for Official Maintainers
Post the link of the official maintainer of the Device Configuration Files for that particular device.
The official maintainers forum link should be from XDA Developers Community Forum.

#### Requirements
1. Should contain configuration files with atleast a general build success rate of 35%
2. Should contain configuration files with atleast a dedicated build success rate of 50%

#### How to include
1. Fork the repository and include the configuration files in bin/Debug/devices folder with appropriate folder structure.
2. The commit message should include the forum link and must start with message **"New Device Support"**.