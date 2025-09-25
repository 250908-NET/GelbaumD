using System.ComponentModel.DataAnnotations;
namespace Barbershop.Models


{
    public enum HaircutType
    {
        Haircut,
        [Display(Name = "Haircut and Beard")]
        HaircutAndBeard,
        [Display(Name = "Shape Up")]
        ShapeUp,
        Shampoo,
        [Display(Name = "The Works")]
        TheWorks
    }
}