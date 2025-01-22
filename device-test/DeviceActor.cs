using System;
using Akka.Actor;
using Akka.Event;

namespace device_test
{
    public class DeviceActor : ReceiveActor
    {
        private readonly ILoggingAdapter _log = Logging.GetLogger(Context);
        private DeviceConfiguration _configuration;

        public DeviceActor()
        {
            Become(Initiated);
        }

        private void Initiated()
        {
            _log.Info("PaymentActor before Initiated {0}", Self.Path);

            // Get configuration from configuration lookup service (urls)

            // create DeviceTwinActor

            Receive<DeviceTwinResponse>(msg =>
            {
                // Set device configuration

                Become(Connecting);
            });
        }

        private void Connecting()
        {
            _log.Info("PaymentActor before Initiated {0}", Self.Path);
            
            // Prepare info for handshake (e.g. certificate)

            // Make a handshake request to DeviceTwinActor
            
            Receive<HandshakeResponse>(msg =>
            {
                // Set cloud tags configuration

                Become(Ready);
            });
        }

        private void Ready()
        {
            Receive<CloudTagChangeRequest>(msg => {
                // when cloud tag configuraion chnaged
            });

            Receive<DeviceConfigurationRequest>(msg => {
                // when device configuration changes
            });

            Receive<RemoteControlRequest>(msg => {
                // request to change cloud tag value
            });
        }
    }
}