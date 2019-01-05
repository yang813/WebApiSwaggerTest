using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using Dapper;

namespace Sample05
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Hello World!");
            //test_insert();
            test_csrediscore();
            Console.ReadKey();
        }


        static void test_high_csrediscore()
        {
            //普通订阅
            RedisHelper.Subscribe(
              ("chan1", msg => Console.WriteLine(msg.Body)),
              ("chan2", msg => Console.WriteLine(msg.Body)));

            //模式订阅（通配符）
            RedisHelper.PSubscribe(new[] { "test*", "*test001", "test*002" }, msg => {
                Console.WriteLine($"PSUB   {msg.MessageId}:{msg.Body}    {msg.Pattern}: chan:{msg.Channel}");
            });
            //模式订阅已经解决的难题：
            //1、分区的节点匹配规则，导致通配符最大可能匹配全部节点，所以全部节点都要订阅
            //2、本组 "test*", "*test001", "test*002" 订阅全部节点时，需要解决同一条消息不可执行多次

            //发布
            RedisHelper.Publish("chan1", "123123123");
            //无论是分区或普通模式，RedisHelper.Publish 都可以正常通信



        }

        /// <summary>
        /// CSRedisCore基础测试
        /// </summary>
        static void test_csrediscore()
        {
            //普通模式
            var csredis = new CSRedis.CSRedisClient("127.0.0.1:6379,password=123456,defaultDatabase=csRedisCoreTest,poolsize=50,ssl=false,writeBuffer=10240");
            //初始化
            RedisHelper.Initialization(csredis);

            RedisHelper.Set("name", "yangjb");//设置值。默认永不过期
            //RedisHelper.SetAsync("name", "祝雷");//异步操作
            Console.WriteLine(RedisHelper.Get<String>("name"));

            RedisHelper.Set("time", DateTime.Now, 1);
            Console.WriteLine(RedisHelper.Get<DateTime>("time"));
            Thread.Sleep(1100);
            Console.WriteLine(RedisHelper.Get<DateTime>("time"));

            // 列表
            RedisHelper.RPush("list", "第一个元素");
            RedisHelper.RPush("list", "第二个元素");
            RedisHelper.LInsertBefore("list", "第二个元素", "我是新插入的第二个元素！");
            Console.WriteLine($"list的长度为{RedisHelper.LLen("list")}");
            //Console.WriteLine($"list的长度为{RedisHelper.LLenAsync("list")}");//异步
            Console.WriteLine($"list的第二个元素为{RedisHelper.LIndex("list", 1)}");
            //Console.WriteLine($"list的第二个元素为{RedisHelper.LIndexAsync("list",1)}");//异步
            // 哈希
            RedisHelper.HSet("person", "name", "zhulei");
            RedisHelper.HSet("person", "sex", "男");
            RedisHelper.HSet("person", "age", "28");
            RedisHelper.HSet("person", "adress", "hefei");
            Console.WriteLine($"person这个哈希中的age为{RedisHelper.HGet<int>("person", "age")}");
            //Console.WriteLine($"person这个哈希中的age为{RedisHelper.HGetAsync<int>("person", "age")}");//异步


            // 集合
            RedisHelper.SAdd("students", "zhangsan", "lisi");
            RedisHelper.SAdd("students", "wangwu");
            RedisHelper.SAdd("students", "zhaoliu");
            Console.WriteLine($"students这个集合的大小为{RedisHelper.SCard("students")}");
            Console.WriteLine($"students这个集合是否包含wagnwu:{RedisHelper.SIsMember("students", "wangwu")}");

        }

        /// <summary>
        /// 测试插入单条数据
        /// </summary>
        static void test_insert()
        {
            var content = new Content
            {
                title = "标题1",
                content = "内容1",

            };
            using (var conn = new SqlConnection("Data Source=127.0.0.1;User ID=sa;Password=bao13253858716;Initial Catalog=CMS;Pooling=true;Max Pool Size=100;"))
            {
                string sql_insert = @"INSERT INTO [Content]
                (title, [content], status, add_time, modify_time)
                     VALUES   (@title,@content,@status,@add_time,@modify_time)";
                var result = conn.Execute(sql_insert, content);
                Console.WriteLine($"test_insert：插入了{result}条数据！");
            }
        }

        /// <summary>
        /// 测试一次批量插入两条数据
        /// </summary>
        static void test_mult_insert()
        {
            List<Content> contents = new List<Content>() {
               new Content
            {
                title = "批量插入标题1",
                content = "批量插入内容1",

            },
               new Content
            {
                title = "批量插入标题2",
                content = "批量插入内容2",

            },
        };

            using (var conn = new SqlConnection("Data Source=127.0.0.1;User ID=sa;Password=1;Initial Catalog=Czar.Cms;Pooling=true;Max Pool Size=100;"))
            {
                string sql_insert = @"INSERT INTO [Content]
                (title, [content], status, add_time, modify_time)
                  VALUES   (@title,@content,@status,@add_time,@modify_time)";
                var result = conn.Execute(sql_insert, contents);
                Console.WriteLine($"test_mult_insert：插入了{result}条数据！");
            }
        }

        /// <summary>
        /// 测试删除单条数据
        /// </summary>
        static void test_del()
        {
            var content = new Content
            {
                id = 2,

            };
            using (var conn = new SqlConnection("Data Source=127.0.0.1;User ID=sa;Password=1;Initial Catalog=Czar.Cms;Pooling=true;Max Pool Size=100;"))
            {
                string sql_insert = @"DELETE FROM [Content]
                     WHERE   (id = @id)";
                var result = conn.Execute(sql_insert, content);
                Console.WriteLine($"test_del：删除了{result}条数据！");
            }
        }

        /// <summary>
        /// 测试一次批量删除两条数据
        /// </summary>
        static void test_mult_del()
        {
            List<Content> contents = new List<Content>() {
               new Content
            {
                id=3,

            },
               new Content
            {
                id=4,

            },
        };

            using (var conn = new SqlConnection("Data Source=127.0.0.1;User ID=sa;Password=1;Initial Catalog=Czar.Cms;Pooling=true;Max Pool Size=100;"))
            {
                string sql_insert = @"DELETE FROM [Content]
                     WHERE   (id = @id)";
                var result = conn.Execute(sql_insert, contents);
                Console.WriteLine($"test_mult_del：删除了{result}条数据！");
            }
        }

        /// <summary>
        /// 测试修改单条数据
        /// </summary>
        static void test_update()
        {
            var content = new Content
            {
                id = 5,
                title = "标题5",
                content = "内容5",

            };
            using (var conn = new SqlConnection("Data Source=127.0.0.1;User ID=sa;Password=1;Initial Catalog=Czar.Cms;Pooling=true;Max Pool Size=100;"))
            {
                string sql_insert = @"UPDATE  [Content]
                    SET         title = @title, [content] = @content, modify_time = GETDATE()
                    WHERE   (id = @id)";
                var result = conn.Execute(sql_insert, content);
                Console.WriteLine($"test_update：修改了{result}条数据！");
            }
        }

        /// <summary>
        /// 测试一次批量修改多条数据
        /// </summary>
        static void test_mult_update()
        {
            List<Content> contents = new List<Content>() {
               new Content
            {
                id=6,
                title = "批量修改标题6",
                content = "批量修改内容6",

            },
               new Content
            {
                id =7,
                title = "批量修改标题7",
                content = "批量修改内容7",

            },
        };

            using (var conn = new SqlConnection("Data Source=127.0.0.1;User ID=sa;Password=1;Initial Catalog=Czar.Cms;Pooling=true;Max Pool Size=100;"))
            {
                string sql_insert = @"UPDATE  [Content]
                    SET         title = @title, [content] = @content, modify_time = GETDATE()
                    WHERE   (id = @id)";
                var result = conn.Execute(sql_insert, contents);
                Console.WriteLine($"test_mult_update：修改了{result}条数据！");
            }
        }

        /// <summary>
        /// 查询单条指定的数据
        /// </summary>
        static void test_select_one()
        {
            using (var conn = new SqlConnection("Data Source=127.0.0.1;User ID=sa;Password=1;Initial Catalog=Czar.Cms;Pooling=true;Max Pool Size=100;"))
            {
                string sql_insert = @"select * from [dbo].[content] where id=@id";
                var result = conn.QueryFirstOrDefault<Content>(sql_insert, new { id = 5 });
                Console.WriteLine($"test_select_one：查到的数据为：");
            }
        }

        /// <summary>
        /// 查询多条指定的数据
        /// </summary>
        static void test_select_list()
        {
            using (var conn = new SqlConnection("Data Source=127.0.0.1;User ID=sa;Password=1;Initial Catalog=Czar.Cms;Pooling=true;Max Pool Size=100;"))
            {
                string sql_insert = @"select * from [dbo].[content] where id in @ids";
                var result = conn.Query<List<Content>>(sql_insert, new { ids = new int[] { 6, 7 } });
                Console.WriteLine($"test_select_one：查到的数据为：");
            }
        }
        /// <summary>
        /// 关联查询
        /// </summary>
        static void test_select_content_with_comment()
        {
            using (var conn = new SqlConnection("Data Source=127.0.0.1;User ID=sa;Password=1;Initial Catalog=Czar.Cms;Pooling=true;Max Pool Size=100;"))
            {
                string sql_insert = @"select * from content where id=@id;
                     select * from comment where content_id=@id;";
                using (var result = conn.QueryMultiple(sql_insert, new { id = 5 }))
                {
                    var content = result.ReadFirstOrDefault<ContentWithComment>();
                    content.comments = result.Read<Comment>();
                    Console.WriteLine($"test_select_content_with_comment:内容5的评论数量{content.comments.Count()}");
                }

            }
        }
    }
}
