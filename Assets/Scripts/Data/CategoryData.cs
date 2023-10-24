using System;
using UnityEngine;
using Sirenix.OdinInspector;

[Serializable] public class CategoryData
{
    public Category Category => _category;

    private string _categoryName = "Category";
    [FoldoutGroup("@_categoryName"), SerializeField, HideLabel, OnValueChanged(nameof(SetCategoryName),true)] 
    private Category _category = null;

    private void SetCategoryName()
    {
        _categoryName = _category.CategoryType.ToString();
    }
}