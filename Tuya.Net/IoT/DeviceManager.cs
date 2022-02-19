﻿using Newtonsoft.Json;
using Tuya.Net.Data;

namespace Tuya.Net.IoT
{
    internal class DeviceManager : IDeviceManager
    {
        private readonly ITuyaClient client;

        public DeviceManager(ITuyaClient client)
        {
            this.client = client;
        }

        /// <inheritdoc />
        public async Task<Device?> GetDeviceAsync(string deviceId, IAccessToken? accessToken = default, CancellationToken ct = default)
        {
            return await client.AuthenticatedRequestAsync<Device?>(HttpMethod.Get, $"/v1.0/devices/{deviceId}", accessToken, cancellationToken: ct);
        }

        /// <inheritdoc />
        public async Task<DeviceInfo?> GetDeviceInfoAsync(string deviceId, IAccessToken? accessToken = default, CancellationToken ct = default)
        {
            return await client.AuthenticatedRequestAsync<DeviceInfo?>(HttpMethod.Get, $"/v1.1/iot-03/devices/{deviceId}", accessToken, cancellationToken: ct);
        }

        /// <inheritdoc />
        public async Task<IList<DeviceStatus>?> GetDeviceStatusAsync(DeviceInfo device, IAccessToken? accessToken = default, CancellationToken ct = default)
        {
            ThrowIfInvalid(device);
            return await GetDeviceStatusAsync(device.Id!, accessToken, ct);
        }

        /// <inheritdoc />
        public async Task<IList<DeviceStatus>?> GetDeviceStatusAsync(string deviceId, IAccessToken? accessToken = default, CancellationToken ct = default)
        {
            return await client.AuthenticatedRequestAsync<IList<DeviceStatus>?>(HttpMethod.Get, $"/v1.0/devices/{deviceId}/status", accessToken, cancellationToken: ct);
        }

        /// <inheritdoc />
        public async Task<IList<Device>?> GetDevicesByUserAsync(User user, IAccessToken? accessToken = default, CancellationToken cancellationToken = default)
        {
            ThrowIfInvalid(user);
            return await GetDevicesByUserAsync(user.Id!, accessToken, cancellationToken);
        }

        /// <inheritdoc />
        public async Task<IList<Device>?> GetDevicesByUserAsync(string userId, IAccessToken? accessToken = default, CancellationToken ct = default)
        {
            return await client.AuthenticatedRequestAsync<IList<Device>?>(HttpMethod.Get, $"/v1.0/users/{userId}/devices", accessToken, cancellationToken: ct);
        }

        /// <inheritdoc />
        public async Task<InstructionInfo?> GetDeviceInstructionsAsync(string deviceId, IAccessToken? accessToken = default, CancellationToken ct = default)
        {
            return await client.AuthenticatedRequestAsync<InstructionInfo?>(HttpMethod.Get, $"/v1.0/devices/{deviceId}/functions", accessToken, cancellationToken: ct);
        }

        /// <inheritdoc />
        public async Task<InstructionInfo?> GetDeviceInstructionsAsync(DeviceInfo device, IAccessToken? accessToken = default, CancellationToken ct = default)
        {
            ThrowIfInvalid(device);
            return await GetDeviceInstructionsAsync(device.Id!, accessToken, ct);
        }

        /// <inheritdoc />
        public async Task<bool> SendCommandAsync(DeviceInfo device, Command command, IAccessToken? accessToken = default, CancellationToken ct = default)
        {
            ThrowIfInvalid(device);
            return await SendCommandAsync(device.Id!, command, accessToken, ct);
        }

        /// <inheritdoc />
        public async Task<bool> SendCommandAsync(string deviceId, Command command, IAccessToken? accessToken = default, CancellationToken ct = default)
        {
            var commands = new List<Command>() { command };
            return await SendCommandListAsync(deviceId, commands, accessToken, ct);
        }

        /// <inheritdoc />
        public async Task<bool> SendCommandListAsync(DeviceInfo device, IList<Command> commands, IAccessToken? accessToken = default, CancellationToken ct = default)
        {
            ThrowIfInvalid(device);
            return await SendCommandListAsync(device.Id!, commands, accessToken, ct);
        }

        /// <inheritdoc />
        public async Task<bool> SendCommandListAsync(string deviceId, IList<Command> commands, IAccessToken? accessToken = default, CancellationToken ct = default)
        {
            return await client.AuthenticatedRequestAsync<bool>(HttpMethod.Post, $"/v1.0/devices/{deviceId}/commands", accessToken, JsonConvert.SerializeObject(new { commands }), cancellationToken: ct);
        }

        /// <summary>
        /// Helper method to throw if a passed Tuya identifiable object is null or its ID is null.
        /// </summary>
        /// <param name="tuyaObject">The Tuya object.</param>
        /// <exception cref="ArgumentNullException">Thrown if the passed Tuya object is null, or its ID is null.</exception>
        private static void ThrowIfInvalid(IIdentifiable tuyaObject)
        {
            ArgumentNullException.ThrowIfNull(tuyaObject);
            ArgumentNullException.ThrowIfNull(tuyaObject.Id);
        }
    }
}