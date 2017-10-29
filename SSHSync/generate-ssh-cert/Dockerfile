FROM debian

RUN apt-get update && apt-get install ssh -y

VOLUME /usr/src/app
WORKDIR /usr/src/app

ENV targetUser=
ENV targetHost=

CMD ssh-keygen -t rsa -f /usr/src/app/id_rsa && \
    ssh $targetUser@$targetHost 'cat >> .ssh/authorized_keys' < /usr/src/app/id_rsa.pub
