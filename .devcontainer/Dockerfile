FROM mcr.microsoft.com/dotnet/sdk:10.0

ARG USERNAME=vscode
ARG USER_UID=1000
ARG USER_GID=$USER_UID

ENV DOTNET_CLI_TELEMETRY_OPTOUT=1 \
    DOTNET_NOLOGO=1 \
    DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=0

RUN apt-get update \
    && apt-get install -y --no-install-recommends \
        ca-certificates \
        curl \
        git \
        icu-devtools \
        libicu-dev \
        procps \
        sudo \
    && if getent group ${USER_GID} >/dev/null; then \
        EXISTING_GROUP=$(getent group ${USER_GID} | cut -d: -f1); \
      else \
        groupadd --gid ${USER_GID} ${USERNAME}; \
        EXISTING_GROUP=${USERNAME}; \
      fi \
    && if getent passwd ${USER_UID} >/dev/null; then \
        EXISTING_USER=$(getent passwd ${USER_UID} | cut -d: -f1); \
      else \
        useradd --uid ${USER_UID} --gid ${USER_GID} -m ${USERNAME}; \
        EXISTING_USER=${USERNAME}; \
      fi \
    && if [ "${EXISTING_USER}" != "${USERNAME}" ]; then usermod -l ${USERNAME} ${EXISTING_USER}; fi \
    && if [ "${EXISTING_GROUP}" != "${USERNAME}" ]; then groupmod -n ${USERNAME} ${EXISTING_GROUP}; fi \
    && usermod -aG sudo ${USERNAME} \
    && echo "${USERNAME} ALL=(root) NOPASSWD:ALL" > /etc/sudoers.d/${USERNAME} \
    && chmod 0440 /etc/sudoers.d/${USERNAME} \
    && rm -rf /var/lib/apt/lists/*

USER ${USERNAME}

WORKDIR /workspaces/VxFormGenerator
