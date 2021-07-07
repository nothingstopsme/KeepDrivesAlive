# KeepDrivesAlive
This is an alternative programme (and rather simplified one) to [this](https://github.com/stsrki/KeepAliveHD), which is used to thwart the undesired power-saving/standby mode automatically activated by your external hard-drive controller.

## Rationale
It has been observed that my old external drive incurred a noticeable amount of seeking operations resuling from the periodic file-writing as the trick to force it to stay responsive; therefore it would be great if my drive can suffer less from those extra costs and gain a longer lifespan, while the same purpose is still fulfilled. 

## Features
To reduce the overheads and increase efficiency, a different approach is taken highlighting the following designs
1. Instead of file-writing, an io-control command, [IOCTL_DISK_UPDATE_PROPERTIES](https://docs.microsoft.com/en-us/windows/win32/api/winioctl/ni-winioctl-ioctl_disk_update_properties), is utilised to nudge drives. Note that this io-control directly targets drives themselves, not the partitions within as file-writing does.
2. The nudge call (the io-contorl command IOCTL_DISK_UPDATE_PROPERTIES) is only issued to a drive when it is deemed *inactive*, which is determined via the monitoring of its read/write counts obtained through another io-control command, [IOCTL_DISK_PERFORMANCE](https://docs.microsoft.com/en-us/windows/win32/api/winioctl/ni-winioctl-ioctl_disk_performance).
