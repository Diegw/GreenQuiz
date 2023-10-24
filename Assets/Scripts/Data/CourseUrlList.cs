using System;
using System.Collections.Generic;
using UnityEngine;
    
[Serializable] public class CourseUrl
{
    public EMenuCourse CourseType => _course;
    public string URL => _url;

    [SerializeField] private EMenuCourse _course;
    [SerializeField] private string _url;
}

[Serializable] public class CourseUrlList
{
    [SerializeField] private List<CourseUrl> _courseUrlList;

    public string GetUrl(EMenuCourse courseType)
    {
        return _courseUrlList.Find(x => x.CourseType == courseType).URL;
    }
}
