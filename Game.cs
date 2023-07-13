using System;


public class Game{
    static int[] size = new int[] {(int)5,(int)5};
    static int[,] maps = new int[size[0],size[1]];

    static System.Random rnd = new System.Random((int)System.DateTime.UtcNow.Ticks);


    bool keep=true;
    public Game(){
        // for(int i=0;i<2;i++){
        //     int tmp=rnd.Next(size[0]*size[1]);
        //     maps[tmp/size[0],tmp%size[0]]=2;
        // }
        maps[0,0]=2;
        maps[1,0]=2;
    }

    public void printMaps(){
        Console.WriteLine();
        for(int i=0;i<size[0];i++){
            for(int j=0;j<size[1];j++){
                Console.Write(maps[i,j].ToString()+'\t');
            }
            Console.WriteLine('\n');
        }
    }

    /*
    orientation=0:
        bias=0~size[1]
        index[0]=i~i
        index[1]=j~j+bias
    orientation=1:
        bias=0~size[0]
        index[0]=i~bias
        index[1]=j~j
    */
    // check if element at maps[i,j] is mergeable, if true return pair position
    Tuple<bool,int,int> mergeAble(int i, int j, int orientation){
        int max_bias=orientation==1?size[0]-i:size[1]-j;
        for(int bias=1;bias<max_bias;bias++){
            if(maps[i+bias*orientation,j-bias*(orientation-1)]!=0){
                return Tuple.Create<bool,int,int>(maps[i+bias*orientation,j-bias*(orientation-1)]==maps[i,j],i+bias*orientation,j-bias*(orientation-1));
            }
        }
        return Tuple.Create<bool,int,int>(false,-1,-1);
    }

    //orientation=0 for horizontal, =1 for axial
    //merge (i2,j2) to (i1,j1)
    void subMerge(int i1,int j1,int i2,int j2){
        maps[i1,j1]*=2;
        maps[i2,j2]=0;
    }

    //orientation=0 for horizontal, =1 for axial
    public void merge(int orientation){
        for(int i=0;i<size[0];i++){
            for(int j=0;j<size[1];j++){
                Tuple<bool,int,int> tmp=mergeAble(i,j,orientation);
                if(tmp.Item1){
                    subMerge(i,j,tmp.Item2,tmp.Item3);
                }
            }
        }
    }

    void generate(){
        List<int> tmpX=new List<int>();
        List<int> tmpY=new List<int>();
        for(int i=0;i<size[0];i++){
            for(int j=0;j<size[1];j++){
                if(maps[i,j]==0){
                    tmpX.Add(i);
                    tmpY.Add(j);
                }
            }
        }
        int gen_idx=rnd.Next(tmpX.Count());
        maps[tmpX.ElementAt(gen_idx),tmpY.ElementAt(gen_idx)]=2;
    }
    

    // 1.merge
    // 2.push
    // 3.generate
    // 4.print map
    /*
    orientation:
        (00) push to top
            j=0~size[1] i=0~size[0]
        (01) push to left
            i=0~size[0] j=0~size[1]
        (10) push to buttom
            j=0~size[1] i=size[0]-1~-1
        (11) push to left
            i=0~size[0] j=size[1]-1~-1
    */
    void roundOperation(string cmd){
        switch(cmd){
            case "w":
                merge(1);
                for(int j=0;j<size[1];j++){
                    List<int> tmp = new List<int>();
                    for(int i=0;i<size[0];i++){
                        if(maps[i,j]!=0){
                            tmp.Add(maps[i,j]);
                        }
                    }
                    for(int i=tmp.Count();i<size[0];i++){
                        tmp.Add(0);
                    }
                    for(int i=0;i<size[0];i++){
                        maps[i,j]=tmp.ElementAt(i);
                    }
                }
                generate();
                break;
            case "a":
                merge(0);
                for(int i=0;i<size[0];i++){
                    List<int> tmp = new List<int>();
                    for(int j=0;j<size[1];j++){
                        if(maps[i,j]!=0){
                            tmp.Add(maps[i,j]);
                        }
                    }
                    for(int j=tmp.Count();j<size[1];j++){
                        tmp.Add(0);
                    }
                    for(int j=0;j<size[1];j++){
                        maps[i,j]=tmp.ElementAt(j);
                    }
                }
                generate();
                break;
            case "s":
                merge(1);
                for(int j=0;j<size[1];j++){
                    List<int> tmp = new List<int>();
                    for(int i=0;i<size[0];i++){
                        if(maps[i,j]!=0){
                            tmp.Add(maps[i,j]);
                        }
                    }
                    for(int i=0;i<size[0]-tmp.Count();i++){
                        maps[i,j]=0;
                    }
                    for(int i=size[0]-tmp.Count();i<size[0];i++){
                        maps[i,j]=tmp.ElementAt(i-size[0]+tmp.Count());
                    }
                }
                generate();
                break;
            case "d":
                merge(0);
                for(int i=0;i<size[0];i++){
                    List<int> tmp = new List<int>();
                    for(int j=0;j<size[1];j++){
                        if(maps[i,j]!=0){
                            tmp.Add(maps[i,j]);
                        }
                    }
                    for(int j=0;j<size[1]-tmp.Count();j++){
                        maps[i,j]=0;
                    }
                    for(int j=size[1]-tmp.Count();j<size[1];j++){
                        maps[i,j]=tmp.ElementAt(j-size[1]+tmp.Count());
                    }
                }
                generate();
                break;
            case "q":
                keep=false;
                return;
            default:
                Console.WriteLine("unknown command, please press in w/a/s/d");
                break;
        }
        printMaps();
    }

    public void start(){
        Console.WriteLine("press w/a/s/d to play\n");
        printMaps();
        string cmd;
        while(keep){
            cmd=Console.ReadLine();
            roundOperation(cmd);
        }
    }
    

}