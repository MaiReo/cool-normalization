namespace Cool.Normalization.Permissions
{
    public class NullPermissionManager : IPermissionManager
    {

        public void Register()
        {
            //No actions.
        }

        public static NullPermissionManager Instance 
            => new NullPermissionManager();
    }
}
