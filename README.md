# ET-StateSync-DEMO
仅供学习交流.  
<b>此Demo是本人抽空学习研究和理论实现，完成时间较短，缺少完善流程以及模块化封装，仅供参考，如有问题请联系QQ：470667444</b>   

## 项目简介
本工程的目标是在ET框架的基础上,做一个状态同步的DEMO.   
借以学习服务器端开发.和了解服务器/客户端交互细节.  

## 特别感谢
1.双端工程基于开源框架ET所写   
传送门: https://github.com/egametang/ET    
2.服务端AOI使用 @初见 开源的ET-Module
传送门：https://github.com/qq362946/AOI

## 项目特点   
1.状态同步  
2.客户端状态插值+预测  

## 状态同步原理参考
如何实现确定性的网络同步：https://www.jianshu.com/p/c1fb23afbabe
服务器将状态同步给客户端(状态缓存,状态插值,估算帧)：https://www.jianshu.com/p/6c1b37735c85
客户端本地预表现：https://www.jianshu.com/p/5dbdf81c4e69
网络游戏同步法则：http://www.skywind.me/blog/archives/112
影子跟随算法：http://www.skywind.me/blog/archives/1145

## 未来发展
1.AOI状态同步（玩家视野区域创建，移除，更新其他玩家）
2.网络同步技能框架（前后端通用）
3.服务端延迟补偿（优先级搁置）
