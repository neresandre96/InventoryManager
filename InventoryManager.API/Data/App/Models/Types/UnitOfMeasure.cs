using System;
using System.Reflection;

namespace InventoryManager.API.Data.App.Models.Types
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class UnitAttribute : Attribute
    {
        public string Name { get; }
        public string Abbreviation { get; }

        public UnitAttribute(string name, string abbreviation)
        {
            Name = name;
            Abbreviation = abbreviation;
        }
    }

    public enum UnitOfMeasure
    {
        [Unit("Grams", "g")]
        Grams,
        [Unit("Kilograms", "kg")]
        Kilograms,
        [Unit("Milliliters", "ml")]
        Milliliters,
        [Unit("Liters", "L")]
        Liters
    }

    public static class UnitOfMeasureExtensions
    {
        public static string GetName(this UnitOfMeasure unit)
        {
            var attribute = GetUnitAttribute(unit);
            return attribute?.Name ?? unit.ToString();
        }

        public static string GetAbbreviation(this UnitOfMeasure unit)
        {
            var attribute = GetUnitAttribute(unit);
            return attribute?.Abbreviation ?? unit.ToString();
        }

        private static UnitAttribute GetUnitAttribute(UnitOfMeasure unit)
        {
            var fieldInfo = unit.GetType().GetField(unit.ToString());
            return fieldInfo?.GetCustomAttribute<UnitAttribute>();
        }
    }
}
