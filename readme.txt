
一、需要提交文档列表：
1、数据库字典
2、数据接口文档
3、业务流程
4、用户操作手册

二、目录文件说明：
TrainOne.Lib.DAL					----数据访问层
TrainOne.One.BLL					----数据业务层
TrainOne.One.Model					----数据实体
		Models						----与数据表一一对应实体
		Dto							----数据输出实体
		Extend						----内部数据处理实体
TrainOne.Lib.Common					----通用帮助类
TrainOne.One.Admin					----后台管理Web端
		/wwwroot/LayuiAdmin			----layui样式
		/views/docktor
TrainOne.One.Api					----数据接口


三、
api地址：1808.api.lcvue.com

四、git错误信息
错误信息：OpenSSL SSL_read: Connection was aborted, errno 10053
解决方案：项目文件夹的cmd中输入 git config --global http.sslVerify false