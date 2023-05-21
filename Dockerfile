FROM mono:latest
RUN mkdir /opt/app
COPY ./CigarsAndGunsHouse/bin/Debug/CigarsAndGunsHouse.exe /opt/app
CMD ["mono", "/opt/app/CigarsAndGunsHouse.exe"]