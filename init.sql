#若基础和处理后数据均存文件，则此表不用
DROP TABLE IF EXISTS `t_base_data`;
CREATE TABLE `t_base_data` (
  `id` bigint(18) NOT NULL AUTO_INCREMENT,
  `equip_id` varchar(20) DEFAULT NULL COMMENT '设备号',
  `base_data` blob COMMENT '基础数据',
  `time` datetime DEFAULT NULL COMMENT '采样时间',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

#设备指令日志
DROP TABLE IF EXISTS `t_cmd_log`;
CREATE TABLE `t_cmd_log` (
  `id` bigint(18) NOT NULL AUTO_INCREMENT,
  `cmd_type` varchar(20) DEFAULT NULL COMMENT '指令类型',
  `cmd_code` varchar(10) DEFAULT NULL COMMENT '指令代码',
  `send_to` varchar(20) DEFAULT NULL COMMENT '发送到的设备号',
  `status` char(1) DEFAULT NULL COMMENT '是否成功 0失败 1成功',
  `send_date` datetime DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP COMMENT '发送日期',
  `send_by` varchar(20) DEFAULT NULL COMMENT '发送人',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

#设备信息
DROP TABLE IF EXISTS `t_equip_info`;
CREATE TABLE `t_equip_info` (
  `id` bigint(18) NOT NULL AUTO_INCREMENT,
  `equip_no` varchar(20) DEFAULT NULL COMMENT '设备编号',
  `position` varchar(30) DEFAULT NULL COMMENT '设备位置',
  `lon` decimal(12,6) DEFAULT NULL COMMENT '经度',
  `lat` decimal(12,6) DEFAULT NULL COMMENT '纬度',
  `ip` varchar(13) DEFAULT NULL COMMENT 'IP',
  `status` char(1) DEFAULT NULL COMMENT '使用状态   0未使用 1使用中  ',
  `is_online` char(1) DEFAULT NULL COMMENT '是否在线  0离线  1在线',
  `product_fac` varchar(40) DEFAULT NULL COMMENT '生产厂家',
  `freq` int(5) DEFAULT NULL COMMENT '采样频率',
  `range` double(10,4) DEFAULT NULL COMMENT '量程',
  `pass` varchar(20) DEFAULT NULL COMMENT '使用通道',
  `desc` varchar(100) DEFAULT NULL COMMENT '描述',
  PRIMARY KEY (`id`),
  KEY `id` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

#同基础数据表
DROP TABLE IF EXISTS `t_handled_data`;
CREATE TABLE `t_handled_data` (
  `id` bigint(18) NOT NULL AUTO_INCREMENT,
  `equip_id` bigint(18) DEFAULT NULL COMMENT '设备ID',
  `base_data_id` bigint(18) DEFAULT NULL COMMENT '基础数据ID  基础数据会隔段时间删除，所以非必填',
  `handled_data` blob COMMENT '处理后数据',
  `lon` double(10,6) DEFAULT NULL,
  `lat` double(10,6) DEFAULT NULL,
  `need_warning` char(1) DEFAULT NULL COMMENT '是否需要预警   0不需要  1需要',
  `create_time` datetime DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP COMMENT '生成时间',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

#处理后需预警数据表
DROP TABLE IF EXISTS `t_handled_data_warn`;
CREATE TABLE `t_handled_data_warn` (
  `id` bigint(18) NOT NULL AUTO_INCREMENT,
  `handled_data_id` bigint(18) DEFAULT NULL COMMENT '处理后数据ID（若按照100台设备考虑，数据量很大所以把需要预警的数据放这边）',
  `handled_data` blob COMMENT '数据',
  `millis` bigint(15) DEFAULT NULL COMMENT '时间戳   毫秒数',
  `lon` double(10,6) DEFAULT NULL COMMENT '经度',
  `lat` double(10,6) DEFAULT NULL COMMENT '纬度',
  `need_warn` char(1) DEFAULT NULL COMMENT '1需要预警 0不需要',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8;

#操作日志
DROP TABLE IF EXISTS `t_operate_log`;
CREATE TABLE `t_operate_log` (
  `id` bigint(18) NOT NULL AUTO_INCREMENT,
  `op_type` varchar(20) DEFAULT NULL COMMENT '操作类型',
  `op_by` varchar(20) DEFAULT NULL COMMENT '操作人',
  `op_date` datetime DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP COMMENT '操作时间',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

#资源
DROP TABLE IF EXISTS `t_resource`;
CREATE TABLE `t_resource` (
  `id` bigint(18) NOT NULL AUTO_INCREMENT,
  `pid` bigint(18) DEFAULT NULL COMMENT '父级ID',
  `res_name` varchar(10) DEFAULT NULL,
  `res_code` varchar(10) DEFAULT NULL,
  `res_type` char(1) DEFAULT NULL COMMENT '类型   1菜单   2功能  3操作',
  `res_sort` int(4) DEFAULT NULL COMMENT '菜单排序',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

#角色
DROP TABLE IF EXISTS `t_role`;
CREATE TABLE `t_role` (
  `id` bigint(18) NOT NULL AUTO_INCREMENT,
  `role_name` varchar(20) DEFAULT NULL COMMENT '角色名',
  `desc` varchar(255) DEFAULT NULL COMMENT '角色描述',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

#角色资源关联表
DROP TABLE IF EXISTS `t_role_resource`;
CREATE TABLE `t_role_resource` (
  `role_id` bigint(18) DEFAULT NULL,
  `res_id` bigint(18) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

#边坡信息
DROP TABLE IF EXISTS `t_sideslope_info`;
CREATE TABLE `t_sideslope_info` (
  `id` bigint(18) NOT NULL AUTO_INCREMENT,
  `lon` double(12,6) DEFAULT NULL COMMENT '经度',
  `lat` double(12,6) DEFAULT NULL,
  `length` double(10,4) DEFAULT NULL,
  `width` double(10,4) DEFAULT NULL,
  `every_side` varchar(200) DEFAULT NULL COMMENT '每条边的长度角度',
  `slope_angle` double(10,4) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

#用户
DROP TABLE IF EXISTS `t_user`;
CREATE TABLE `t_user` (
  `id` bigint(18) NOT NULL AUTO_INCREMENT,
  `user_name` varchar(20) NOT NULL COMMENT '用户名',
  `password` varchar(36) NOT NULL COMMENT '密码',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

#角色
DROP TABLE IF EXISTS `t_user_role`;
CREATE TABLE `t_user_role` (
  `user_id` bigint(18) NOT NULL,
  `role_id` bigint(18) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
