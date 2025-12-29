namespace PupilFramework.UI
{
    public enum UIMaskType
    {
        //半透明
        Half,

        //完全透明
        Complete,

        //完全透明的基础上也不阻挡射线
        NoEffect
    }

    public enum UIEffectType
    {
        //隐藏上一级
        DisableUpperLevel,

        //暂停上一级
        PauseUpperLevel,

        //隐藏其他
        DisableOthers
    }

    public enum UIShowType
    {
        //全屏  此模式下
        //遮罩默认不自动响应出栈
        FullScreen,

        //弹窗
        PopScreen,
    }

    public class Model_UIDataModel
    {
        //遮罩是否响应自动出栈
        public bool IsMaskBack { get; set; }

        //显示类型
        public UIShowType Show { get; set; }

        //遮罩透明度
        public UIMaskType Mask { get; set; }

        //处理上一层级
        public UIEffectType Effect { get; set; }
    }
}