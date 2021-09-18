namespace AmSoul.FPC1020
{
    /// <summary>
    /// 通讯包Parket识别代码
    /// </summary>
    public enum ParketCode
    {
        /// <summary>
        /// 命令包Command packet
        /// </summary>
        CMD_PREFIX_CODE = 0xAA55,
        /// <summary>
        /// 指令数据包Command Data Packet
        /// </summary>
        CMD_DATA_PREFIX_CODE = 0xA55A,
        /// <summary>
        /// 响应包Response packet
        /// </summary>
        RCM_PREFIX_CODE = 0x55AA,
        /// <summary>
        /// 响应数据包Response Data Packet
        /// </summary>
        RCM_DATA_PREFIX_CODE = 0x5AA5
    }
    /// <summary>
    /// 命令列表 Command List
    /// </summary>
    public enum CommandCode
    {
        #region System Code (0x0000 ~ 0x001F, 0x0000 : Reserved)
        /// <summary>
        /// 进行与设备的通讯测试
        /// For test the communication to device
        /// </summary>
        CMD_TEST_CONNECTION = 0x0001,
        /// <summary>
        /// 设置设备参数 (Device ID, Security Level, Baudrate, Duplication Check, Auto Learn,TimeOut)
        /// 注：TimeOut只适用于滑动采集器
        /// Set parameter (Device ID, Security Level, BPS, Duplication Check ,Auto Learn)
        /// </summary>
        CMD_SET_PARAM = 0x0002,
        /// <summary>
        /// 获取设备参数 (Device ID, Security Level, Baudrate, Duplication Check, Auto Learn，TimeOut)
        /// 注：TimeOut只适用于滑动采集器
        /// Get parameter (Device ID, Security Level, BPS, Duplication Check, Auto Learn)
        /// </summary>
        CMD_GET_PARAM = 0x0003,
        /// <summary>
        /// 获取设备信息
        /// Get device information
        /// </summary>
        CMD_GET_DEVICE_INFO = 0x0004,
        /// <summary>
        /// 将设备设置为 IAP状态
        /// Set device to IAP mode
        /// </summary>
        CMD_ENTER_IAP_MODE = 0x0005,
        /// <summary>
        /// Set the ID Note to appointed ID number from host to module;
        /// Note: Temporarily does not support
        /// </summary>
        CMD_SET_ID_NOTE = 0x0006,
        /// <summary>
        /// Get the ID Note that appointed ID number from module to Host;
        /// Note: Temporarily does not support
        /// </summary>
        CMD_GET_ID_NOTE = 0x0007,
        /// <summary>
        /// 在设备中设置模块序列号信息（Module SN）
        /// Set the Module SN from Host and to Module ; 
        /// Note: Temporarily does not support
        /// </summary>
        CMD_SET_MODULE_SN = 0x0008,
        /// <summary>
        /// 获取本设备的模块序列号（ Module SN）
        /// Get the Module SN from module to Host ;
        /// Note: Temporarily does not support
        /// </summary>
        CMD_GET_MODULE_SN = 0x0009,
        /// <summary>
        /// 
        /// </summary>
        CMD_SET_DEVPASS = 0x000A,
        /// <summary>
        /// 
        /// </summary>
        CMD_VERIFY_DEVPASS = 0x000B,
        /// <summary>
        /// 使模块进入休眠状态
        /// 注：有些模块不支持休眠功能，虽然模块响应该指令返回成功
        /// </summary>
        CMD_ENTER_STANDY_STATE = 0x000C,
        /// <summary>
        /// 
        /// </summary>
        CMD_UPGRADE_FIRMWARE = 0x000D,
        #endregion

        #region Sensor Code (0x0020 ~ 0x003F)
        /// <summary>
        /// 从采集器采集指纹图像并保存于 ImageBuffer 中
        /// Capture the fingerprint image and save to ImageBuffer 
        /// </summary>
        CMD_GET_IMAGE = 0x0020,
        /// <summary>
        /// 检测指纹输入状态
        /// Detect the fingerprint input state
        /// </summary>
        CMD_FINGER_DETECT = 0x0021,
        /// <summary>
        /// 将保存于 ImageBuffer 中的指纹图像上传至HOST
        /// Upload the fingerprint image data in ImageBuffer to HOST
        /// </summary>
        CMD_UP_IMAGE = 0x0022,
        /// <summary>
        /// HOST下载指纹图像到模块的ImageBuffer 中
        /// Download the fingerprint image to module and save in ImageBuffer 
        /// </summary>
        CMD_DOWN_IMAGE = 0x0023,
        /// <summary>
        /// 控制采集器背光灯的开/关（注：半导体传感器不用此功能）
        /// Control sensor light（only for optical sensor, semiconductor sensor no this function）
        /// </summary>
        CMD_SLED_CTRL = 0x0024,
        /// <summary>
        /// 
        /// </summary>
        CMD_ADJUST_SENSOR = 0x0025,
        #endregion

        #region Template Code (0x0040 ~ 0x005F)
        /// <summary>
        /// 将指定编号Ram Buffer中的Template，注册到指定编号的库中
        /// Save the template data that storage in Ram Buffer into Flash Memory
        /// </summary>
        CMD_STORE_CHAR = 0x0040,
        /// <summary>
        /// 读取库中指定编号中的Template到指定编号的 Ram Buffer
        /// Save the template data that storage in flash memory into Ram Buffer 
        /// </summary>
        CMD_LOAD_CHAR = 0x0041,
        /// <summary>
        /// 将保存于指定编号的Ram Buffer 中的 Template 上传至HOST
        /// Upload the template data that storage in appointed Ram Buffer to HOST
        /// </summary>
        CMD_UP_CHAR = 0x0042,
        /// <summary>
        /// 从HOST下载 Template到模块指定编号的 Ram Buffer 中
        /// Download the template data to appointed Ram Buffer in module
        /// </summary>
        CMD_DOWN_CHAR = 0x0043,
        /// <summary>
        /// 删除指定编号范围内的 Template
        /// Appointed an range in DB and then delete all template data in the range
        /// </summary>
        CMD_DEL_CHAR = 0x0044,
        /// <summary>
        /// 获取指定范围内可注册的（没有注册的）第一个模板编号
        /// appointed an range and get the first ID number which is unappropriated in the range
        /// </summary>
        CMD_GET_EMPTY_ID = 0x0045,
        /// <summary>
        /// 获取指定编号的模板注册状态
        /// Get the appointed ID number whether is available
        /// </summary>
        CMD_GET_STATUS = 0x0046,
        /// <summary>
        /// 检查指定编号范围内的所有指纹模板是否存在坏损的情况
        /// Check all the template data whether or not broken in the appointed range
        /// </summary>
        CMD_GET_BROKEN_ID = 0x0047,
        /// <summary>
        /// 获取指定编号范围内已注册的模板个数
        /// Get the count of enrolled template in appointed range
        /// </summary>
        CMD_GET_ENROLL_COUNT = 0x0048,
        /// <summary>
        /// 获取已注册User ID列表
        /// </summary>
        CMD_GET_ENROLLED_ID_LIST = 0x0049,
        #endregion

        #region FingerPrint Alagorithm Code (0x0060 ~ 0x007F)
        /// <summary>
        /// 将ImageBuffer 中的指纹图像生成模板数据，并保存于指定编号的 Ram Buffer 中
        /// Generate template data from fingerprint image that saved in ImageBuffer and then save to appointed Ram Buffer
        /// </summary>
        CMD_GENERATE = 0x0060,
        /// <summary>
        /// 将保存于Ram Buffer 中的两或三个模板数据融合成一个模板数据
        /// Compose two or three template that saved in Ram Buffer and then generate ultimate template data
        /// </summary>
        CMD_MERGE = 0x0061,
        /// <summary>
        /// 指定 Ram Buffer 中的两个指纹模板之间进行 1:1 比对
        /// 1:1 match, match between two templates that appointed in Ram Buffer
        /// </summary>
        CMD_MATCH = 0x0062,
        /// <summary>
        /// 指定 Ram Buffer 中的模板与指纹库中指定编号范围内的所有模板之间进行 1:N 比对
        /// 1:N identify ,an template in Ram Buffer match to all template that appointed range in DB
        /// </summary>
        CMD_SEARCH = 0x0063,
        /// <summary>
        /// 指定 Ram Buffer 中的指纹模板与指纹库中指定编号的指纹模板之间进行 1:1比对
        /// 1:1 match, an template in Ram Buffer match with an template in DB
        /// </summary>
        CMD_VERIFY = 0x0064,
        /// <summary>
        /// Unknown Command
        /// </summary>
        RCM_INCORRECT_COMMAND = 0x00FF
        #endregion
    }
    /// <summary>
    /// 错误代码表 Error Code
    /// </summary>
    public enum ErrorCode
    {
        /// <summary>
        /// 指令处理成功
        /// Command dispose success
        /// </summary>
        ERR_SUCCESS = 0x00,
        /// <summary>
        /// 指令处理失败
        /// Command dispose failed 
        /// </summary>
        ERR_FAIL = 0x01,
        /// <summary>
        /// 
        /// </summary>
        //ERR_CONNECTION = 2,
        /// <summary>
        /// 与指定编号中 Template 的 1:1比对失败
        /// 1:1 match failed with appointed ID Template.
        /// </summary>
        ERR_VERIFY = 0x10,
        /// <summary>
        /// 已进行 1:N 比对，但相同 Template 不存在
        /// 1:N match，but no the same Template .
        /// </summary>
        ERR_IDENTIFY = 0x11,
        /// <summary>
        /// 在指定编号中不存在已注册的 Template
        /// No enrolled Template in appointed ID.
        /// </summary>
        ERR_TMPL_EMPTY = 0x12,
        /// <summary>
        /// 在指定编号中已存在 Template
        /// With template data in appointed ID.
        /// </summary>
        ERR_TMPL_NOT_EMPTY = 0x13,
        /// <summary>
        /// 不存在已注册的 Template
        /// NO any enrolled template data
        /// </summary>
        ERR_ALL_TMPL_EMPTY = 0x14,
        /// <summary>
        /// 不存在可注册的 Template ID 
        /// No valid for enroll Template ID .
        /// </summary>
        ERR_EMPTY_ID_NOEXIST = 0x15,
        /// <summary>
        /// 不存在已损坏的 Template
        /// No broken Template.
        /// </summary>
        ERR_BROKEN_ID_NOEXIST = 0x16,
        /// <summary>
        /// 指定的 Template Data 无效
        /// Appointed template Data invalid.
        /// </summary>
        ERR_INVALID_TMPL_DATA = 0x17,
        /// <summary>
        /// 该指纹已注册
        /// This fingerprint have been enrolled.
        /// </summary>
        ERR_DUPLICATION_ID = 0x18,
        /// <summary>
        /// 指纹图像质量不好
        /// Fingerprint image lower quality .
        /// </summary>
        ERR_BAD_QUALITY = 0x19,
        /// <summary>
        /// Template 合成失败
        /// Template merge failed.
        /// </summary>
        ERR_MERGE_FAIL = 0x1A,
        /// <summary>
        /// 没有进行通讯密码确认
        /// 注：
        /// 1.若已设有通讯密码但没有调用 CMD_VERIFY_DEVPASS 进行确认，则除了CMD_TEST_CONNECTION, CMD_VERIFY_DEVPASS之外的所有指令都返回该错误码。
        /// 2.若没有设置通讯密码，则可以不经过确认密码就可以使用所有指令。
        /// </summary>
        ERR_NOT_AUTHORIZED = 0x1B,
        /// <summary>
        /// 外部Flash 烧写出错
        /// Memory error .
        /// </summary>
        ERR_MEMORY = 0x1C,
        /// <summary>
        /// 指定 Template 编号无效
        /// Template ID No. that appointed is invalid.
        /// </summary>
        ERR_INVALID_TMPL_NO = 0x1D,
        /// <summary>
        /// 使用了不正确的参数
        /// Use wrong parameters .
        /// </summary>
        ERR_INVALID_PARAM = 0x22,
        /// <summary>
        /// 在TimeOut时间内没有输入指纹
        /// </summary>
        ERR_TIME_OUT = 0x23,
        /// <summary>
        /// 指纹合成个数无效
        /// Count of template merge is invalid.
        /// </summary>
        ERR_GEN_COUNT = 0x25,
        /// <summary>
        /// Buffer ID 值不正确
        /// Buffer ID No. is wrong
        /// </summary>
        ERR_INVALID_BUFFER_ID = 0x26,
        /// <summary>
        /// 
        /// </summary>
        ERR_INVALID_OPERATION_MODE = 0x27,
        /// <summary>
        /// 采集器上没有指纹输入
        /// No fingerprint in sensor .
        /// </summary>
        ERR_FP_NOT_DETECTED = 0x28,
        /// <summary>
        /// 指令被取消
        /// </summary>
        ERR_FP_CANCEL = 0x41
    }
    public enum GD_CODE
    {
        GD_TEMPLATE_NOT_EMPTY = 0x01,
        GD_TEMPLATE_EMPTY = 0x00

    }
}
