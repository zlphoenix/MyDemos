namespace TelChina.TRF.Persistant.CoreLib.Entity
{
    internal interface IPersistableObject
    {
        void SetDefaultValue();

        void OnValidate();
    }
}
