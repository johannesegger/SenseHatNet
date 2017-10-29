FROM debian

RUN apt-get update && \
    apt-get install ssh rsync -y

ENV targetUser=
ENV targetHost=
ENV targetDir=
VOLUME /usr/src/app
WORKDIR /usr/src/app

# Don't mount it directly to `/root/.ssh` because of permission issues
VOLUME /usr/src/ssh

CMD cp -r /usr/src/ssh /root/.ssh && \
    chmod 0600 /root/.ssh/id_rsa && \
    rsync -aivz /usr/src/app/* $targetUser@$targetHost:$targetDir
