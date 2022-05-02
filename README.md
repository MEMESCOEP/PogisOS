# Pogis Operating System
[![Build Status](https://travis-ci.org/joemccann/dillinger.svg?branch=master)](https://github.com/MEMESCOEP/PogisOS)<br/>
Pogis Operating System is a basic command-line OS.
## Features
- DHCP Network Auto-configuration
- Basic FAT32 Filesystem
- Simple text editor
- Auto filesystem mounting

## Usage
In order to use PogisOS, you'll need to follow these steps:
### Requirements:
- USB Flash drive (128 MB or larger)
- PogisOS Disk Image (Located in the 'Releases' tab)
- Rufus (https://rufus.ie/en/)

### Flashing PogisOS
1. Download the latest PogisOS disk image (if you haven't already)
2. Download Rufus (if you haven't already)
3. Insert your USB Flash drive into your computer, and select it from the dropdown in rufus.
4. In Rufus, select the PogisOS disk image.
5. Click the start button at the bottom of the window. (:warning:WARNING:warning: This step will ERASE ALL DATA ON YOUR USB FLASH DRIVE! Make sure you are okay with this before you proceed!)
6. Wait for rufus to finish
7. You're Done!

### Booting on Real Hardware
_Before you can boot on real hardware,  you need to flash PogisOS to a USB Flash drive. Take a look at the "Flashing PogisOS" section for instructions._ 

1. Once you've flashed PogisOS, Insert your USB Flash drive into the computer you want to boot from.
2. Power on your computer and press the BIOS boot options key (This could be any key, so look up your key) until the boot menu appears.
3. Select your USB Flash drive form the list using the arrow keys, and press enter.
4. Your computer should now be booting into PogisOS!
