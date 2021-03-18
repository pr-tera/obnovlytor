using System.ServiceProcess;

namespace obnovlytor
{
    class Service
    {
        internal static bool Status()
        {
            bool _check = false;
            if (!string.IsNullOrEmpty(Reqistry.AgentServiceName()))
            {
                try
                {
                    ServiceController service = new ServiceController(Reqistry.AgentServiceName());
                    switch (service.Status)
                    {
                        case ServiceControllerStatus.Stopped:
                            _check = false;
                            break;
                        case ServiceControllerStatus.Running:
                            _check = true;
                            break;
                        case ServiceControllerStatus.StopPending:
                            _check = false;
                            break;
                        case ServiceControllerStatus.ContinuePending:
                            _check = false;
                            break;
                        case ServiceControllerStatus.Paused:
                            _check = false;
                            break;
                        case ServiceControllerStatus.PausePending:
                            _check = false;
                            break;
                        case ServiceControllerStatus.StartPending:
                            _check = true;
                            break;
                    }
                }
                catch
                {
                    _check = false;
                }
            }
            else
            {
                _check = false;
            }
            return _check;
        }
    }
}
