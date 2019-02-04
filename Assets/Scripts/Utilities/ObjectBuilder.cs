using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

// Create object using reflection。All properties should be "string". Data will be convert to different data types automatically.
public class ObjectBuilder
{
    private string _objectName;
    private List<KeyValuePair<string, string>> _propertyList;

    public ObjectBuilder(string objectName)
    {
        this._objectName = objectName;
        this._propertyList = new List<KeyValuePair<string, string>>();
    }

    public ObjectBuilder SetProperty(string key, string value)
    {
        _propertyList.Add(new KeyValuePair<string, string>(key, value));
        return this;
    }

    public System.Object Build()
    {
        //創建實體
        Type type = Type.GetType(_objectName);
        Debug.Log("builder: " + _objectName);
        System.Object obj = Activator.CreateInstance(type);

        //填入數據
        foreach (KeyValuePair<string, string> pair in _propertyList)
        {
            //根據名稱取出 getter method
            PropertyInfo propertyInfo = type.GetProperty(pair.Key);
            MethodInfo getterInfos = propertyInfo.GetGetMethod();
            Type returnType = getterInfos.ReturnType;

            System.Object parameter = null;

            //只支援基本類型轉換 (int, float, string, double, bool)
            if (returnType.Equals(typeof(int)))
            {
                parameter = int.Parse(pair.Value);
            } 
            else if (returnType.Equals(typeof(float)))
            {
                parameter = float.Parse(pair.Value);
            }
            else if (returnType.Equals(typeof(string)))
            {
                parameter = pair.Value;
            }
            else if (returnType.Equals(typeof(double)))
            {
                parameter = double.Parse(pair.Value);
            }
            else if (returnType.Equals(typeof(bool)))
            {
                parameter = bool.Parse(pair.Value);
            }
            else if (returnType.Equals(typeof(int[])))
            {
                string[] data = pair.Value.Split(',');
                List<int> result = new List<int>();
                foreach (string item in data)
                {
                    result.Add(int.Parse(item));
                }
                parameter = result.ToArray();
            }
            else if (returnType.Equals(typeof(float[])))
            {
                string[] data = pair.Value.Split(',');
                List<float> result = new List<float>();
                foreach (string item in data)
                {
                    result.Add(float.Parse(item));
                }
                parameter = result.ToArray();
            }
            else if (returnType.Equals(typeof(double[])))
            {
                string[] data = pair.Value.Split(',');
                List<double> result = new List<double>();
                foreach (string item in data)
                {
                    result.Add(double.Parse(item));
                }
                parameter = result.ToArray();
            }
            else if (returnType.Equals(typeof(bool[])))
            {
                string[] data = pair.Value.Split(',');
                List<bool> result = new List<bool>();
                foreach (string item in data)
                {
                    result.Add(bool.Parse(item));
                }
                parameter = result.ToArray();
            }
            else
            {
                Debug.LogError("Unsupported effect parameter, name: " +_objectName + ", key: " + pair.Key);
            }

            //根據名稱取出 setter method 並呼叫
            MethodInfo setterInfos = propertyInfo.GetSetMethod();
            setterInfos.Invoke(obj, new object[] { parameter });
        }

        return obj;
    }
}
